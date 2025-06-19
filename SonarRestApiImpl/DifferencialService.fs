module DifferencialService

open FSharp.Data
open SonarRestService.Types
open SonarRestServiceImpl
open System
open SonarRestService
open System.Threading

type ComponentTreeSearch = JsonProvider<""" {"paging":{"pageIndex":1,"pageSize":100,"total":9},"baseComponent":{"key":"Tekla.Structures.Core:TeklaStructuresWPF","name":"TeklaStructuresWPF","qualifier":"TRK","measures":[]},"components":[{"key":"Tekla.Structures.Core:TeklaStructuresWPF:CommonObjects/SectionMark/ViewSharingStatusProperty.cs","name":"ViewSharingStatusProperty.cs","qualifier":"FIL","path":"CommonObjects/SectionMark/ViewSharingStatusProperty.cs","language":"cs","measures":[{"metric":"new_conditions_to_cover","period":{"index":1,"value":"8"}},{"metric":"new_coverage","period":{"index":1,"value":"0.0","bestValue":false}},{"metric":"new_lines_to_cover","period":{"index":1,"value":"20"}},{"metric":"new_uncovered_conditions","period":{"index":1,"value":"8","bestValue":false}},{"metric":"new_uncovered_lines","period":{"index":1,"value":"20","bestValue":false}}]},{"key":"Tekla.Structures.Core:TeklaStructuresWPF:TeklaStructuresLauncher/Controllers/NewModelController.cs","name":"NewModelController.cs","qualifier":"FIL","path":"TeklaStructuresLauncher/Controllers/NewModelController.cs","language":"cs","measures":[{"metric":"new_conditions_to_cover","period":{"index":1,"value":"0"}},{"metric":"new_coverage","period":{"index":1,"value":"14.3","bestValue":false}},{"metric":"new_lines_to_cover","period":{"index":1,"value":"7"}},{"metric":"new_uncovered_conditions","period":{"index":1,"value":"0","bestValue":true}},{"metric":"new_uncovered_lines","period":{"index":1,"value":"6","bestValue":false}}]},{"key":"Tekla.Structures.Core:TeklaStructuresWPF:TeklaStructuresLauncher/Helpers/EnvironmentHelpers.cs","name":"EnvironmentHelpers.cs","qualifier":"FIL","path":"TeklaStructuresLauncher/Helpers/EnvironmentHelpers.cs","language":"cs","measures":[{"metric":"new_conditions_to_cover","period":{"index":1,"value":"12"}},{"metric":"new_coverage","period":{"index":1,"value":"25.0","bestValue":false}},{"metric":"new_lines_to_cover","period":{"index":1,"value":"56"}},{"metric":"new_uncovered_conditions","period":{"index":1,"value":"11","bestValue":false}},{"metric":"new_uncovered_lines","period":{"index":1,"value":"40","bestValue":false}}]},{"key":"Tekla.Structures.Core:TeklaStructuresWPF:TeklaStructuresLauncher/Types/WebSocketMessage.cs","name":"WebSocketMessage.cs","qualifier":"FIL","path":"TeklaStructuresLauncher/Types/WebSocketMessage.cs","language":"cs","measures":[{"metric":"new_conditions_to_cover","period":{"index":1,"value":"0"}},{"metric":"new_coverage","period":{"index":1,"value":"33.3","bestValue":false}},{"metric":"new_lines_to_cover","period":{"index":1,"value":"3"}},{"metric":"new_uncovered_conditions","period":{"index":1,"value":"0","bestValue":true}},{"metric":"new_uncovered_lines","period":{"index":1,"value":"2","bestValue":false}}]},{"key":"Tekla.Structures.Core:TeklaStructuresWPF:TeklaStructuresLauncher/Models/ServiceManagerProvider.cs","name":"ServiceManagerProvider.cs","qualifier":"FIL","path":"TeklaStructuresLauncher/Models/ServiceManagerProvider.cs","language":"cs","measures":[{"metric":"new_conditions_to_cover","period":{"index":1,"value":"6"}},{"metric":"new_coverage","period":{"index":1,"value":"57.1","bestValue":false}},{"metric":"new_lines_to_cover","period":{"index":1,"value":"1"}},{"metric":"new_uncovered_conditions","period":{"index":1,"value":"3","bestValue":false}},{"metric":"new_uncovered_lines","period":{"index":1,"value":"0","bestValue":true}}]},{"key":"Tekla.Structures.Core:TeklaStructuresWPF:TeklaStructuresLauncher/Managers/SandBoxManager.cs","name":"SandBoxManager.cs","qualifier":"FIL","path":"TeklaStructuresLauncher/Managers/SandBoxManager.cs","language":"cs","measures":[{"metric":"new_conditions_to_cover","period":{"index":1,"value":"2"}},{"metric":"new_coverage","period":{"index":1,"value":"66.7","bestValue":false}},{"metric":"new_lines_to_cover","period":{"index":1,"value":"4"}},{"metric":"new_uncovered_conditions","period":{"index":1,"value":"0","bestValue":true}},{"metric":"new_uncovered_lines","period":{"index":1,"value":"2","bestValue":false}}]},{"key":"Tekla.Structures.Core:TeklaStructuresWPF:CommonObjects/Dimension/DimensionLabelsProperty.cs","name":"DimensionLabelsProperty.cs","qualifier":"FIL","path":"CommonObjects/Dimension/DimensionLabelsProperty.cs","language":"cs","measures":[{"metric":"new_conditions_to_cover","period":{"index":1,"value":"2"}},{"metric":"new_coverage","period":{"index":1,"value":"100.0","bestValue":true}},{"metric":"new_lines_to_cover","period":{"index":1,"value":"1"}},{"metric":"new_uncovered_conditions","period":{"index":1,"value":"0","bestValue":true}},{"metric":"new_uncovered_lines","period":{"index":1,"value":"0","bestValue":true}}]},{"key":"Tekla.Structures.Core:TeklaStructuresWPF:TeklaStructuresWPF/Main/FusionSettingsBuilder.cs","name":"FusionSettingsBuilder.cs","qualifier":"FIL","path":"TeklaStructuresWPF/Main/FusionSettingsBuilder.cs","language":"cs","measures":[{"metric":"new_conditions_to_cover","period":{"index":1,"value":"0"}},{"metric":"new_coverage","period":{"index":1,"value":"100.0","bestValue":true}},{"metric":"new_lines_to_cover","period":{"index":1,"value":"1"}},{"metric":"new_uncovered_conditions","period":{"index":1,"value":"0","bestValue":true}},{"metric":"new_uncovered_lines","period":{"index":1,"value":"0","bestValue":true}}]},{"key":"Tekla.Structures.Core:TeklaStructuresWPF:TeklaStructuresWPF/Main/MessageHub.cs","name":"MessageHub.cs","qualifier":"FIL","path":"TeklaStructuresWPF/Main/MessageHub.cs","language":"cs","measures":[{"metric":"new_conditions_to_cover","period":{"index":1,"value":"0"}},{"metric":"new_coverage","period":{"index":1,"value":"100.0","bestValue":true}},{"metric":"new_lines_to_cover","period":{"index":1,"value":"1"}},{"metric":"new_uncovered_conditions","period":{"index":1,"value":"0","bestValue":true}},{"metric":"new_uncovered_lines","period":{"index":1,"value":"0","bestValue":true}}]}]} """>
type PeriodsResponse = JsonProvider<""" {"component":{"key":"Tekla.Structures.Core:TeklaStructuresWPF","name":"TeklaStructuresWPF","qualifier":"TRK","measures":[{"metric":"lines","value":"195639"}]},"period":{"index":1,"mode":"PREVIOUS_VERSION","date":"2025-06-11T16:34:23+0000","parameter":"PI25Q2"}} """>
type CoverageReportType = JsonProvider<""" {"component":{"key":"Tekla.Structures.Core:TeklaStructuresWPF","name":"TeklaStructuresWPF","qualifier":"TRK","measures":[{"metric":"minor_violations","value":"3622","bestValue":false},{"metric":"complexity","value":"19219"},{"metric":"new_coverage","period":{"index":1,"value":"25.8","bestValue":false}},{"metric":"new_info_violations","period":{"index":1,"value":"8","bestValue":false}},{"metric":"sqale_index","value":"13038","bestValue":false},{"metric":"blocker_violations","value":"0","bestValue":true},{"metric":"lines_to_cover","value":"37749"},{"metric":"uncovered_conditions","value":"8148","bestValue":false},{"metric":"major_violations","value":"2405","bestValue":false},{"metric":"new_violations","period":{"index":1,"value":"31","bestValue":false}},{"metric":"new_technical_debt","period":{"index":1,"value":"435","bestValue":false}},{"metric":"new_blocker_violations","period":{"index":1,"value":"0","bestValue":true}},{"metric":"ncloc","value":"127153"},{"metric":"new_conditions_to_cover","period":{"index":1,"value":"30"}},{"metric":"lines","value":"195639"},{"metric":"coverage","value":"66.5","bestValue":false},{"metric":"new_minor_violations","period":{"index":1,"value":"15","bestValue":false}},{"metric":"ncloc_language_distribution","value":"cs\u003d97753;xml\u003d29400"},{"metric":"new_lines","period":{"index":1,"value":"280"}},{"metric":"violations","value":"8102","bestValue":false},{"metric":"new_uncovered_conditions","period":{"index":1,"value":"22","bestValue":false}},{"metric":"critical_violations","value":"104","bestValue":false},{"metric":"new_major_violations","period":{"index":1,"value":"5","bestValue":false}},{"metric":"info_violations","value":"1971","bestValue":false},{"metric":"new_critical_violations","period":{"index":1,"value":"3","bestValue":false}},{"metric":"uncovered_lines","value":"10617","bestValue":false},{"metric":"cognitive_complexity","value":"11497","bestValue":false},{"metric":"new_uncovered_lines","period":{"index":1,"value":"70","bestValue":false}},{"metric":"conditions_to_cover","value":"18219"},{"metric":"new_lines_to_cover","period":{"index":1,"value":"94"}}]}} """>
let GetLeakPeriodStartDate(conf : ISonarConfiguration, projectIn : Resource, httpconnector : IHttpSonarConnector) =
    let url =
        if conf.SonarVersion < 7.6 then
            sprintf "/api/measures/component?additionalFields=period&componentKey=%s&metricKeys=lines" projectIn.Key
        else
            sprintf "/api/measures/component?additionalFields=period&component=%s&metricKeys=lines" projectIn.Key

    let responsecontent = httpconnector.HttpSonarGetRequest(conf, url)
    let data = PeriodsResponse.Parse(responsecontent)
    data.Period.Date

let GetCoverageReportOnNewCodeOnLeak(conf : ISonarConfiguration, projectIn : Resource, httpconnector : IHttpSonarConnector, token:CancellationToken, logger:IRestLogger) =
    let coverageLeak = new System.Collections.Generic.Dictionary<string, CoverageDifferencial>()
    let AddComponentToLeak(comp:ComponentTreeSearch.Component, date:DateTime) = 
        let resource = new Resource()
        resource.Key <- comp.Key
        resource.IdString <- comp.Key
        resource.Qualifier <- comp.Qualifier
        resource.Path <- comp.Path
        resource.Name <- comp.Name
        resource.Lang <- comp.Language

        SourceService.GetLinesFromDateInResource(conf, resource, httpconnector, date)

        let covmeas = new CoverageDifferencial()
        covmeas.resource <- resource
        covmeas.Id <- comp.Key
        let newcov = comp.Measures |> Seq.find (fun meas -> meas.Metric = "new_coverage")
        covmeas.NewCoverage <- newcov.Period.Value
        let newcond = comp.Measures |> Seq.find (fun meas -> meas.Metric = "new_uncovered_conditions")
        covmeas.UncoveredConditons <- Convert.ToInt32(newcond.Period.Value)
        let newlines = comp.Measures |> Seq.find (fun meas -> meas.Metric = "new_uncovered_lines")
        covmeas.UncoveredLines <- Convert.ToInt32(newlines.Period.Value)
        let newLinesTocov = comp.Measures |> Seq.find (fun meas -> meas.Metric = "new_lines_to_cover")
        covmeas.NewLinesToCover <- newLinesTocov.Period.Value
        let newConditionsToCover = comp.Measures |> Seq.find (fun meas -> meas.Metric = "new_conditions_to_cover")
        covmeas.NewConditionsToCover <- newConditionsToCover.Period.Value

        if resource.Lines.Count > 0 then
            coverageLeak.Add(comp.Key, covmeas)

    let rec GetComponents(page:int, date:DateTime) =

        let url =
            if conf.SonarVersion < 7.6 then
                sprintf "/api/measures/component_tree?asc=true&ps=100&metricSortFilter=withMeasuresOnly&s=metricPeriod,name&metricSort=new_coverage&metricPeriodSort=1&baseComponentKey=%s&metricKeys=new_coverage,new_uncovered_lines,new_uncovered_conditions,new_conditions_to_cover,new_lines_to_cover&strategy=leaves&p=%i" projectIn.Key page
            else
                sprintf "/api/measures/component_tree?asc=true&ps=100&metricSortFilter=withMeasuresOnly&s=metricPeriod,name&metricSort=new_coverage&metricPeriodSort=1&component=%s&metricKeys=new_coverage,new_uncovered_lines,new_uncovered_conditions,new_conditions_to_cover,new_lines_to_cover&strategy=leaves&p=%i" projectIn.Key page

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
        let startDate = GetLeakPeriodStartDate(conf, projectIn, httpconnector).DateTime
        GetComponents(1, startDate)
    with
    | ex -> logger.ReportMessage("Failed to Collect Coverage Leak: " + ex.Message + " for project: " + projectIn.Key)
    coverageLeak

let GetSummaryProjectReport(conf : ISonarConfiguration, projectIn : Resource, httpconnector : IHttpSonarConnector) =
    let coverageReport = new System.Collections.Generic.Dictionary<string, ProjectSummaryReport>()

    let GetPeriodValueForMeasure(comp:CoverageReportType.Component, metric:string) =
        let data = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = metric)
        if data.IsSome then
            Convert.ToInt64(data.Value.Period.Value.Value)
        else
            int64(-1)

    let GetValueForMeasure(comp:CoverageReportType.Component, metric:string) =
        let data = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = metric)
        if data.IsSome then
            Convert.ToInt64(data.Value.Value.Number.Value)
        else
            int64(-1)

    let AddComponentToReport(comp:CoverageReportType.Component) = 
        let resource = new Resource()
        resource.Key <- comp.Key
        resource.IdString <- comp.Key
        resource.Qualifier <- comp.Qualifier
        resource.Name <- comp.Name

        let summaryReport = new ProjectSummaryReport()
        summaryReport.Resource <- resource
        summaryReport.Id <- comp.Key.ToString()
        let newcov = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "new_coverage")
        if newcov.IsSome then
            summaryReport.NewCoverage <-
                if newcov.Value.Period.IsSome then
                    newcov.Value.Period.Value.Value
                else
                    int64(-1)
        else
            summaryReport.NewCoverage <- -1

        let langdist = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "ncloc_language_distribution")
        if langdist.IsSome then
            summaryReport.LinesOfCodeLangDistribution <- langdist.Value.Value.String.Value

            
        let newcond = comp.Measures |> Seq.tryFind (fun meas -> meas.Metric = "coverage")
        if newcond.IsSome then
            summaryReport.Coverage <- Convert.ToDecimal(newcond.Value.Value.Number.Value)
        else
            summaryReport.Coverage <- -1

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
        summaryReport.BlockerIssues <- GetValueForMeasure(comp, "blocker_violations")
        summaryReport.CriticalIssues <- GetValueForMeasure(comp, "critical_violations")
        summaryReport.MajorIssues <- GetValueForMeasure(comp, "major_violations")
        summaryReport.MinorIssues <- GetValueForMeasure(comp, "minor_violations")
        summaryReport.InfoIssues <- GetValueForMeasure(comp, "info_violations")
        summaryReport.NewBlockerIssues <- GetPeriodValueForMeasure(comp, "new_blocker_violations")
        summaryReport.NewCriticalIssues <- GetPeriodValueForMeasure(comp, "new_critical_violations")
        summaryReport.NewMajorIssues <- GetPeriodValueForMeasure(comp, "new_major_violations")
        summaryReport.NewMinorIssues <- GetPeriodValueForMeasure(comp, "new_minor_violations")
        summaryReport.NewInfoIssues <- GetPeriodValueForMeasure(comp, "new_info_violations")        
        
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

    let url = sprintf "/api/measures/component?%s&metricKeys=blocker_violations,critical_violations,major_violations,minor_violations,info_violations,new_blocker_violations,new_critical_violations,new_major_violations,new_minor_violations,new_info_violations,conditions_to_cover,new_conditions_to_cover,new_lines_to_cover,lines_to_cover,uncovered_lines,new_uncovered_lines,new_uncovered_conditions,uncovered_conditions,new_coverage,ncloc,coverage,new_lines,ncloc_language_distribution,violations,new_violations,new_technical_debt,cognitive_complexity,complexity,sqale_index,lines" compElement

    let responsecontent = httpconnector.HttpSonarGetRequest(conf, url)
    let data = CoverageReportType.Parse(responsecontent)
    AddComponentToReport(data.Component)

    coverageReport
