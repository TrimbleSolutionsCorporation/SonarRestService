module DifferencialService

open FSharp.Data
open SonarRestService.Types
open SonarRestServiceImpl
open System
open SonarRestService
open System.Threading

type ComponentTreeSearch = JsonProvider<""" {"paging":{"pageIndex":1,"pageSize":100,"total":2},"baseComponent":{"id":"e6cebf55-ccc4-4bc1-a27e-7b447c9f724c","key":"Project:ComponentBla","name":"ComponentBla","qualifier":"TRK","measures":[]},"components":[{"id":"AVEzWO92k3Oz8Oa46je4","key":"Project:ComponentBla:Project:ComponentBla:9AC47FE5-B1C8-416A-BFB4-632B7171E031:UndoRedoBlaModel.cs","name":"UndoRedoBlaModel.cs","qualifier":"FIL","path":"UndoRedoBlaModel.cs","language":"cs","measures":[{"metric":"new_uncovered_conditions","periods":[{"index":1,"value":"0"}]},{"metric":"new_uncovered_lines","periods":[{"index":1,"value":"1"}]},{"metric":"new_coverage","periods":[{"index":1,"value":"0.0"}]}]},{"id":"AVEzWO94k3Oz8Oa46jgV","key":"Project:ComponentBla:Project:ComponentBla:86AE155A-EC6F-4975-81F9-773177AD29FF:BlaTreeFeature.cs","name":"BlaTreeFeature.cs","qualifier":"FIL","path":"BlaTreeFeature.cs","language":"cs","measures":[{"metric":"new_uncovered_conditions","periods":[{"index":1,"value":"0"}]},{"metric":"new_uncovered_lines","periods":[{"index":1,"value":"1"}]},{"metric": "ncloc_language_distribution","value": "cxx=616382;py=640;xml=907"},{"metric":"new_coverage","periods":[{"index":1,"value":"83.3333333333333"}]}]}]} """>
type PeriodsResponse = JsonProvider<""" {"component":{"id":"14170a50-b95b-4506-8f1a-19856f187137","key":"Project:ProjectName","name":"ProjectName","qualifier":"TRK","measures":[{"metric":"new_coverage","periods":[{"index":1,"value":"24.5416078984485"}]}]},"periods":[{"index":1,"mode":"previous_version","date":"2012-02-05T00:11:53+0200","parameter":"VersionName"}]} """>
type CoverageReportType = JsonProvider<""" {"component":{"id":"sdafldfjlakshdjfh","key":"Tekla.Structures.DotApps:ComponentCatalog","name":"ComponentCatalog","qualifier":"TRK","measures":[{"metric":"ncloc","value":"23622","periods":[{"index":1,"value":"23"}]},{"metric": "ncloc_language_distribution","value": "cxx=616382;py=640;xml=907"},{"metric":"new_lines","periods":[{"index":1,"value":"62"}]},{"metric":"coverage","value":"62.8","periods":[{"index":1,"value":"11.0"}]},{"metric":"new_coverage","periods":[{"index":1,"value":"73.0769230769231"}]}]}} """>

let GetLeakPeriodStart(conf : ISonarConfiguration, projectIn : Resource, httpconnector : IHttpSonarConnector) =
    let url =
        if conf.SonarVersion < 7.6 then
            sprintf "/api/measures/component?additionalFields=periods&componentKey=%s&metricKeys=lines" projectIn.Key
        else
            sprintf "/api/measures/component?additionalFields=periods&component=%s&metricKeys=lines" projectIn.Key

    let responsecontent = httpconnector.HttpSonarGetRequest(conf, url)
    let data = PeriodsResponse.Parse(responsecontent)
    data.Periods.[0].Date

let GetCoverageReportOnNewCodeOnLeak(conf : ISonarConfiguration, projectIn : Resource, httpconnector : IHttpSonarConnector, token:CancellationToken, logger:IRestLogger) =
    let coverageLeak = new System.Collections.Generic.Dictionary<string, CoverageDifferencial>()
    let AddComponentToLeak(comp:ComponentTreeSearch.Component, date:DateTime) = 
        let resource = new Resource()
        resource.Key <- comp.Key
        resource.IdString <- comp.Id
        resource.Qualifier <- comp.Qualifier
        resource.Path <- comp.Path
        resource.Name <- comp.Name
        resource.Lang <- comp.Language

        SourceService.GetLinesFromDateInResource(conf, resource, httpconnector, date)

        let covmeas = new CoverageDifferencial()
        covmeas.resource <- resource
        covmeas.Id <- comp.Id
        let newcov = comp.Measures |> Seq.find (fun meas -> meas.Metric = "new_coverage")
        covmeas.NewCoverage <- newcov.Periods.[0].Value
        let newcond = comp.Measures |> Seq.find (fun meas -> meas.Metric = "new_uncovered_conditions")
        covmeas.UncoveredConditons <- Convert.ToInt32(newcond.Periods.[0].Value)
        let newlines = comp.Measures |> Seq.find (fun meas -> meas.Metric = "new_uncovered_lines")
        covmeas.UncoveredLines <- Convert.ToInt32(newlines.Periods.[0].Value)
        let newLinesTocov = comp.Measures |> Seq.find (fun meas -> meas.Metric = "new_lines_to_cover")
        covmeas.NewLinesToCover <- newLinesTocov.Periods.[0].Value
        let newConditionsToCover = comp.Measures |> Seq.find (fun meas -> meas.Metric = "new_conditions_to_cover")
        covmeas.NewConditionsToCover <- newConditionsToCover.Periods.[0].Value

        if resource.Lines.Count > 0 then
            coverageLeak.Add(comp.Key, covmeas)

    let rec GetComponents(page:int, date:DateTime) =        
        let url = sprintf "/api/measures/component_tree?asc=true&ps=100&metricSortFilter=withMeasuresOnly&s=metricPeriod,name&metricSort=new_coverage&metricPeriodSort=1&baseComponentKey=%s&metricKeys=new_coverage,new_uncovered_lines,new_uncovered_conditions,new_conditions_to_cover,new_lines_to_cover&strategy=leaves&p=%i" projectIn.Key page
        logger.ReportMessage("Progress: " + url)
        let responsecontent = httpconnector.HttpSonarGetRequest(conf, url)
        let data = ComponentTreeSearch.Parse(responsecontent)

        data.Components |> Seq.iter (fun c -> AddComponentToLeak(c, date))

        if data.Components.Length = data.Paging.PageSize then
            let nextPage = page + 1
            try
                if not(token.IsCancellationRequested) then GetComponents(nextPage, date)
            with
            | ex -> let message = sprintf "Failed Page: %i with %s" nextPage ex.Message
                    logger.ReportMessage(message)

    try
        let startDate = GetLeakPeriodStart(conf, projectIn, httpconnector).DateTime
        GetComponents(1, startDate)
    with
    | ex -> logger.ReportMessage("Failed to Collect Coverage Leak: " + ex.Message + " for project: " + projectIn.Key)
    coverageLeak

let GetSummaryProjectReport(conf : ISonarConfiguration, projectIn : Resource, httpconnector : IHttpSonarConnector) =
    let coverageReport = new System.Collections.Generic.Dictionary<string, ProjectSummaryReport>()

    let GetPeriodValueForMeasure(comp:CoverageReportType.Component, metric:string) =
        let data = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = metric)
        if data.IsSome then
            Convert.ToInt64(data.Value.Periods.[0].Value)
        else
            int64(0)

    let GetValueForMeasure(comp:CoverageReportType.Component, metric:string) =
        let data = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = metric)
        if data.IsSome then
            Convert.ToInt64(data.Value.Value.Number.Value)
        else
            int64(0)

    let AddComponentToReport(comp:CoverageReportType.Component) = 
        let resource = new Resource()
        resource.Key <- comp.Key
        resource.IdString <- comp.Id
        resource.Qualifier <- comp.Qualifier
        resource.Name <- comp.Name

        let summaryReport = new ProjectSummaryReport()
        summaryReport.Resource <- resource
        summaryReport.Id <- comp.Id.ToString()
        let newcov = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "new_coverage")
        if newcov.IsSome then
            summaryReport.NewCoverage <- newcov.Value.Periods.[0].Value

        let langdist = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "ncloc_language_distribution")
        if langdist.IsSome then
            summaryReport.LinesOfCodeLangDistribution <- langdist.Value.Value.String.Value

        let newcond = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "coverage")
        if newcond.IsSome then
            summaryReport.Coverage <- Convert.ToDecimal(newcond.Value.Value.Number.Value)

        summaryReport.NewLines <- GetPeriodValueForMeasure(comp, "new_lines")
        summaryReport.Lines <- GetValueForMeasure(comp, "lines")
        summaryReport.LinesOfCode <- GetValueForMeasure(comp, "ncloc")        
        summaryReport.NewUncoveredConditions <- GetPeriodValueForMeasure(comp, "new_uncovered_conditions")
        summaryReport.UncoveredConditions <- GetValueForMeasure(comp, "uncovered_conditions")
        summaryReport.NewUncoveredLines <- GetPeriodValueForMeasure(comp, "new_uncovered_lines")
        summaryReport.UncoveredLines <- GetValueForMeasure(comp, "uncovered_lines")
        summaryReport.NewConditionsToCover <- GetPeriodValueForMeasure(comp, "new_conditions_to_cover")
        summaryReport.ConditionsToCover <- GetValueForMeasure(comp, "conditions_to_cover")
        summaryReport.NewLinesToCover <- GetPeriodValueForMeasure(comp, "new_lines_to_cover")
        summaryReport.LinesToCover <- GetValueForMeasure(comp, "lines_to_cover")
        summaryReport.Issues <- GetValueForMeasure(comp, "violations")
        summaryReport.NewIssues <- GetPeriodValueForMeasure(comp, "new_violations")
        summaryReport.NewTechnicalDebt <- GetPeriodValueForMeasure(comp, "new_technical_debt")
        summaryReport.TechnicalDebt <- GetValueForMeasure(comp, "sqale_index")
        summaryReport.CognitiveComplexity <- GetValueForMeasure(comp, "cognitive_complexity")
        summaryReport.CyclomaticComplexity <- GetValueForMeasure(comp, "complexity")

        coverageReport.Add(comp.Key, summaryReport)

    let compElement = 
        if conf.SonarVersion < 7.6 then
            sprintf "componentKey=%s" projectIn.Key
        else
            sprintf "component=%s" projectIn.Key

    let url = sprintf "/api/measures/component?%s&metricKeys=conditions_to_cover,new_conditions_to_cover,new_lines_to_cover,lines_to_cover,uncovered_lines,new_uncovered_lines,new_uncovered_conditions,uncovered_conditions,new_coverage,ncloc,coverage,new_lines,ncloc_language_distribution,violations,new_violations,new_technical_debt,cognitive_complexity,complexity,sqale_index,lines" compElement

    let responsecontent = httpconnector.HttpSonarGetRequest(conf, url)
    let data = CoverageReportType.Parse(responsecontent)
    AddComponentToReport(data.Component)

    coverageReport

let GetCoverageReport(conf : ISonarConfiguration, projectIn : Resource, httpconnector : IHttpSonarConnector) =
    let coverageReport = new System.Collections.Generic.Dictionary<string, CoverageReport>()
    let AddComponentToReport(comp:CoverageReportType.Component) = 
        let resource = new Resource()
        resource.Key <- comp.Key
        resource.IdString <- comp.Id
        resource.Qualifier <- comp.Qualifier
        resource.Name <- comp.Name

        let covmeas = new CoverageReport()
        covmeas.resource <- resource
        covmeas.Id <- comp.Id.ToString()
        let newcov = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "new_coverage")
        if newcov.IsSome then
            covmeas.NewCoverage <- newcov.Value.Periods.[0].Value

        let langdist = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "ncloc_language_distribution")
        if langdist.IsSome then
            covmeas.LinesOfCodeLangDistribution <- langdist.Value.Value.String.Value

        let newcond = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "coverage")
        if newcond.IsSome then
            covmeas.Coverage <- Convert.ToDecimal(newcond.Value.Value.Number.Value)

        let newlines = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "new_lines")
        if newlines.IsSome then
            covmeas.NewLines <- Convert.ToInt64(newlines.Value.Periods.[0].Value)
        let lines = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "ncloc")
        if lines.IsSome then
            covmeas.LinesOfCode <- Convert.ToInt64(lines.Value.Value.Number.Value)

        coverageReport.Add(comp.Key, covmeas)

    let compElement = 
        if conf.SonarVersion < 7.6 then
            sprintf "componentKey=%s" projectIn.Key
        else
            sprintf "component=%s" projectIn.Key

    let url = sprintf "/api/measures/component?%s&metricKeys=new_coverage,ncloc,coverage,new_lines,ncloc_language_distribution" compElement
    let responsecontent = httpconnector.HttpSonarGetRequest(conf, url)
    let data = CoverageReportType.Parse(responsecontent)
    AddComponentToReport(data.Component)

    coverageReport