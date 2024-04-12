﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SonarRestService.fs" company="Copyright © 2014 Tekla Corporation. Tekla is a Trimble Company">
//     Copyright (C) 2013 [Jorge Costa, Jorge.Costa@tekla.com]
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
// of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. 
// You should have received a copy of the GNU Lesser General Public License along with this program; if not, write to the Free
// Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// --------------------------------------------------------------------------------------------------------------------
namespace SonarRestServiceImpl

open FSharp.Data
open FSharp.Data.JsonExtensions
open RestSharp
open SonarRestService
open SonarRestService.Types
open System.Collections.ObjectModel
open System
open System.Web
open System.Net
open System.IO
open System.Text.RegularExpressions
open System.Linq

open SonarRestServiceImpl
open System.Threading
open RestSharp.Authenticators

type SonarService(httpconnector : IHttpSonarConnector) = 
    let httpconnector = httpconnector
    let mutable cancelRequest = false

    let (|NotNull|_|) value = 
        if obj.ReferenceEquals(value, null) then None 
        else Some()

    let GetComment(comment : string) =
        if String.IsNullOrEmpty(comment) then
            String.Empty
        else
            "&comment=" + HttpUtility.UrlEncode(comment)

    let getProjectResourcesFromResponseContent(responsecontent : string) = 
        let resources = JsonResourceWithMetrics.Parse(responsecontent)
        let resourcelist = System.Collections.Generic.List<Resource>()
            
        for resource in resources do
            let res = new Resource()
            res.Id <- resource.Id
            res.Date <- resource.Date.DateTime
            res.Key <- resource.Key.Trim()
            res.Lang <- resource.Lang
            res.Lname <- resource.Lname
            res.Name <- resource.Name
            res.Qualifier <- resource.Qualifier
            res.Scope <- resource.Scope
            try
                res.Version <- sprintf "%s" resource.Version
            with
                | ex -> ()

            resourcelist.Add(res)
        resourcelist

    let getResourcesFromResponseContent(responsecontent : string) = 
        let resources = JsonResourceWithMetrics.Parse(responsecontent)
        let resourcelist = System.Collections.Generic.List<Resource>()
            
        for resource in resources do
            try
                let res = new Resource()
                res.Id <- resource.Id
                res.Date <- resource.Date.DateTime
                res.Key <- resource.Key.Trim()

                if not(obj.ReferenceEquals(resource.JsonValue.TryGetProperty("lang"), null)) then
                    res.Lang <- sprintf "%s" resource.Lang

                res.Lname <- resource.Lname
                res.Name <- resource.Name
                res.Qualifier <- resource.Qualifier
                res.Scope <- resource.Scope
                res.IsBranch <- false
                if not(obj.ReferenceEquals(resource.JsonValue.TryGetProperty("branch"), null)) then
                    res.BranchName <- sprintf "%s" resource.Branch
                    res.IsBranch <- true

                if not(obj.ReferenceEquals(resource.JsonValue.TryGetProperty("version"), null)) then
                    res.Version <- sprintf "%s" resource.Version

                let metrics = new System.Collections.Generic.List<Metric>()

                if not(obj.ReferenceEquals(resource.JsonValue.TryGetProperty("msr"), null)) then
                    for metric in resource.Msr do
                        let met = new Metric()

                        met.Key <- metric.Key

                        match metric.JsonValue.TryGetProperty("data") with
                        | NotNull ->
                            met.Data <- metric.Data.Value.Trim()
                        | _ -> ()

                        
                        met.FormatedValue <- sprintf "%f" metric.FrmtVal

                        match metric.JsonValue.TryGetProperty("val") with
                        | NotNull ->
                            met.Val <- metric.Val
                        | _ -> ()

                        metrics.Add(met)

                        res.Metrics <- metrics


                if not(res.IsBranch) then
                    let keyelems = res.Key.Split(':')
                    let nameelems = res.Name.Split(' ')
                    if keyelems.[keyelems.Length - 1] = nameelems.[nameelems.Length - 1] && nameelems.Length > 1 then
                        res.IsBranch <- true
                        res.BranchName <- nameelems.[nameelems.Length - 1]
                    
                resourcelist.Add(res)
            with
                | ex -> ()
        resourcelist

    let getRulesFromResponseContent(responsecontent : string) = 
        let rules = JSonRule.Parse(responsecontent)
        let rulesToReturn = new System.Collections.Generic.List<Rule>()

        for ruleInServer in rules do
            let rule = new Rule()
            rule.Name <- ruleInServer.Title            
            rule.ConfigKey <- ruleInServer.ConfigKey
            rule.HtmlDescription <- ruleInServer.Description
            rule.Key <- ruleInServer.Key
            rule.Severity <- (EnumHelper.asEnum<Severity>(ruleInServer.Priority)).Value
            rule.Repo <- ruleInServer.Plugin
            rulesToReturn.Add(rule)

        rulesToReturn
        
    let GetQualityProfilesFromContent(responsecontent : string, conf : ISonarConfiguration, rest : ISonarRestService) = 
        let parsed = JsonQualityProfiles.Parse(responsecontent)
        let profiles = new System.Collections.Generic.List<Profile>()

        for eachprofile in parsed do
            let newProfile = new Profile(rest, conf)
            newProfile.Default <- eachprofile.Default
            newProfile.Language <- eachprofile.Language
            newProfile.Name <- eachprofile.Name

            let profileRules = new System.Collections.Generic.List<Rule>()
            let profileAlerts = new System.Collections.Generic.List<Alert>()

            profiles.Add(newProfile)

        profiles

    let GetProfileFromContent(responsecontent : string, conf : ISonarConfiguration, rest : ISonarRestService) = 
        let parsed = JSonProfile.Parse(responsecontent)
        let profiles = new System.Collections.Generic.List<Profile>()

        for eachprofile in parsed do
            let newProfile = new Profile(rest, conf)
            newProfile.Default <- eachprofile.Default
            newProfile.Language <- eachprofile.Language
            newProfile.Name <- eachprofile.Name

            let profileRules = new System.Collections.Generic.Dictionary<string, Rule>()
            let profileAlerts = new System.Collections.Generic.List<Alert>()

            for eachrule in eachprofile.Rules do
                let newRule = new Rule()
                newRule.Key <- eachrule.Key
                newRule.Repo <- eachrule.Repo
                newRule.ConfigKey <- eachrule.Key + ":" + eachrule.Repo
                newRule.Severity <- (EnumHelper.asEnum<Severity>(eachrule.Severity)).Value
                if not(profileRules.ContainsKey(newRule.ConfigKey)) then
                    profileRules.Add(newRule.ConfigKey, newRule)

                newProfile.AddRule(newRule)

            try
                for eachalert in eachprofile.Alerts do
                    let newAlert = new Alert()
                    match eachalert.JsonValue.TryGetProperty("error") with
                    | NotNull ->
                        newAlert.Error <- eachalert.Error
                    | _ -> ()

                    match eachalert.JsonValue.TryGetProperty("warning") with
                    | NotNull ->
                        newAlert.Warning <- eachalert.Warning
                    | _ -> ()

                    newAlert.Metric <- eachalert.Metric
                    newAlert.Operator  <- eachalert.Operator
                    profileAlerts.Add(newAlert)

                newProfile.Alerts <- profileAlerts
            with
             | ex -> ()

            profiles.Add(newProfile)

        profiles

    let GetSourceFromContent(content : string) =
        let data = JsonValue.Parse(content)
        let source = new Source()

        let arrayOfLines : string array = Array.zeroCreate (data.[0].Properties |> Seq.length)

        let CreateLine(data : string, elem : JsonValue) =
            let line = new Line()
            arrayOfLines.[Int32.Parse(data) - 1] <- elem.InnerText()
            ()

        data.[0].Properties |> Seq.iter (fun elem -> CreateLine(elem))

        source.Lines <- arrayOfLines
        source

    let GetSourceFromRaw(raw : string) =
        new Source(Lines = Regex.Split(raw, "\r\n|\r|\n"))

    let GetDuplicationsFromContent(responsecontent : string) = 
        let dups = JSonDuplications.Parse(responsecontent)
        let duplicationData = new Collections.Generic.List<DuplicationData>()

        dups |> Seq.iter (fun x ->
                            let duplicatedResource = new DuplicationData()
                            let resource = new Resource(Date = x.Date.DateTime,
                                                        Id = x.Id,
                                                        Key = x.Key,
                                                        Name = x.Name,
                                                        Lname = x.Lname,
                                                        Qualifier = x.Qualifier,
                                                        Scope = x.Scope,
                                                        Lang = x.Lang)
                            
                            duplicatedResource.Resource <- resource

                            let data = DupsData.Parse(x.Msr.[0].Data)
                            for group in data.Gs do
                                let groupToAdd = new DuplicatedGroup()
                                for block in group.Bs do
                                    let blockToAdd = new DuplicatedBlock()
                                    blockToAdd.Lenght <- block.L
                                    blockToAdd.Startline <- block.S
                                    blockToAdd.Resource <- new Resource(Key = block.R)
                                    groupToAdd.DuplicatedBlocks.Add(blockToAdd)
                                duplicatedResource.DuplicatedGroups.Add(groupToAdd)

                            duplicationData.Add(duplicatedResource))
                            

        duplicationData

    let GetCoverageFromContent(responsecontent : string) = 
        let source = new SourceCoverage()

        try
            let resources = JsonResourceWithMetrics.Parse(responsecontent)


            try
                source.SetLineCoverageData(resources.[0].Msr.[0].Data.Value)
            with
                | ex -> ()

            try
                source.SetBranchCoverageData(resources.[0].Msr.[1].Data.Value, resources.[0].Msr.[2].Data.Value)
            with
                | ex -> ()
        with
            | ex -> ()

        source

    let QuerySonar(userconf : ISonarConfiguration, urltosue : string, methodin : Method) =
        let client = new RestClient(userconf.Hostname)
        client.Authenticator <- new HttpBasicAuthenticator(userconf.Username, userconf.Password)
        let request = new RestRequest(urltosue, methodin)
        request.AddHeader(HttpRequestHeader.Accept.ToString(), "text/xml") |> ignore
        client.Execute(request)



    let CreateRuleInProfile2(parsedDataRule:JsonRule.Rule, profile : Profile)=
        let newRule = new Rule()
        newRule.Key <-  try parsedDataRule.InternalKey with | ex -> ""
        newRule.ConfigKey <-  try parsedDataRule.InternalKey with | ex -> ""
        newRule.Repo <- parsedDataRule.Repo
        newRule.Name <- parsedDataRule.Name
        newRule.CreatedAt <- DateTime.Parse(parsedDataRule.CreatedAt)
        newRule.Severity <- try (EnumHelper.asEnum<Severity>(parsedDataRule.Severity)).Value with | ex -> Severity.UNDEFINED
        newRule.Status <- try (EnumHelper.asEnum<Status>(parsedDataRule.Status)).Value with | ex -> Status.UNDEFINED
        newRule.InternalKey <- try parsedDataRule.InternalKey with | ex -> ""
        newRule.IsTemplate <- try parsedDataRule.IsTemplate with | ex -> false
        for tag in parsedDataRule.Tags do
            newRule.Tags.Add(tag)
        for tag in parsedDataRule.SysTags do
            newRule.SysTags.Add(tag)
        newRule.Lang <- parsedDataRule.Lang
        newRule.LangName <- parsedDataRule.LangName
        newRule.HtmlDescription <- parsedDataRule.HtmlDesc
        newRule.DefaultDebtChar <- try (EnumHelper.asEnum<Category>(parsedDataRule.DefaultDebtChar)).Value with | ex -> Category.UNDEFINED
        newRule.DefaultDebtSubChar <- try (EnumHelper.asEnum<SubCategory>(parsedDataRule.DefaultDebtSubChar)).Value with | ex -> SubCategory.UNDEFINED
        newRule.Category <- try (EnumHelper.asEnum<Category>(parsedDataRule.DebtChar)).Value with | ex -> Category.UNDEFINED
        newRule.Subcategory <- try (EnumHelper.asEnum<SubCategory>(parsedDataRule.DebtSubChar)).Value with | ex -> SubCategory.UNDEFINED

        newRule.SubcategoryName <- try parsedDataRule.DebtCharName with | ex -> ""
        newRule.CategoryName <- try parsedDataRule.DebtSubCharName with | ex -> ""                 

        newRule.DefaultDebtRemFnType <- try (EnumHelper.asEnum<RemediationFunction>(parsedDataRule.DefaultDebtRemFnType)).Value with | ex -> RemediationFunction.UNDEFINED
        newRule.DefaultDebtRemFnCoeff <- try parsedDataRule.DefaultDebtRemFnCoeff with | ex -> ""
        newRule.DebtOverloaded <- try parsedDataRule.DebtOverloaded with | ex -> false
        newRule.RemediationFunction <- try (EnumHelper.asEnum<RemediationFunction>(parsedDataRule.DebtRemFnType)).Value with | ex -> RemediationFunction.UNDEFINED
        newRule.DebtRemFnCoeff <- try parsedDataRule.DebtRemFnCoeff with | ex -> ""
        try
            let value = Regex.Replace(newRule.DebtRemFnCoeff, "[^0-9.]", "")
            let unit = newRule.DebtRemFnCoeff.Replace(value, "").Replace("min", "MN").Replace("hour", "H").Replace("day", "D")
            newRule.RemediationFactorVal <- Int32.Parse(value)
            newRule.RemediationFactorTxt <- try (EnumHelper.asEnum<RemediationUnit>(unit)).Value with | ex -> RemediationUnit.UNDEFINED
        with
        | ex ->
            newRule.RemediationFactorVal <- 0
            newRule.RemediationFactorTxt <- RemediationUnit.UNDEFINED

        try
            for param in parsedDataRule.Params do
                let ruleparam  = new RuleParam()
                ruleparam.Key <- param.Key
                let isFound = (List.ofSeq newRule.Params) |> List.tryFind (fun c -> c.Key.Equals(param.Key))
                match isFound with
                | Some elem -> ()
                | _ -> newRule.Params.Add(ruleparam)
        with
        | ex -> ()

        profile.AddRule(newRule)

    let UpdateRuleInProfile(parsedDataRule:JsonRuleSearchResponse.Rule,
                            rule : Rule,
                            skipSeverity : bool,
                            userconf : ISonarConfiguration,
                            profile:Profile) =

        let IfExists(propertyToSet:string) =
            match parsedDataRule.JsonValue.TryGetProperty(propertyToSet) with
            | NotNull ->
                true
            | _ -> 
                false

        if IfExists("repo") then rule.Repo <- parsedDataRule.Repo

        if IfExists("key") then
            rule.Key <-  try parsedDataRule.Key.Replace(rule.Repo  + ":", "") with | ex -> ""
            rule.ConfigKey <-  try parsedDataRule.Key with | ex -> ""


        if IfExists("name") then rule.Name <- parsedDataRule.Name
        if IfExists("type") then rule.Type <- parsedDataRule.Type
        if IfExists("createdAt") then rule.CreatedAt <- DateTime.Parse(parsedDataRule.CreatedAt)
        if not(skipSeverity) then
            if IfExists("severity") then rule.Severity <- try (EnumHelper.asEnum<Severity>(parsedDataRule.Severity)).Value with | ex -> Severity.UNDEFINED

        if IfExists("status") then rule.Status <- try (EnumHelper.asEnum<Status>(parsedDataRule.Status)).Value with | ex -> Status.UNDEFINED
        if IfExists("internalKey") then rule.InternalKey <- parsedDataRule.InternalKey

        if IfExists("isTemplate") then rule.IsTemplate <- try parsedDataRule.IsTemplate with | ex -> false

        if IfExists("tags") then
            for tag in parsedDataRule.Tags do
                let foundAlready = rule.Tags |> Seq.tryFind (fun c -> c.Equals(tag))
                match foundAlready with 
                | Some data -> ()
                | _ -> rule.Tags.Add(tag)
                
        if IfExists("sysTags") then
            for tag in parsedDataRule.SysTags do
                rule.SysTags.Add(tag)

        if IfExists("lang") then rule.Lang <- parsedDataRule.Lang
        if IfExists("langName") then rule.LangName <- parsedDataRule.LangName
        if IfExists("htmlDesc") then rule.HtmlDescription <- parsedDataRule.HtmlDesc
        if IfExists("defaultDebtChar") then rule.DefaultDebtChar <- try (EnumHelper.asEnum<Category>(parsedDataRule.DefaultDebtChar)).Value with | ex -> Category.UNDEFINED
        if IfExists("defaultDebtSubChar") then rule.DefaultDebtSubChar <- try (EnumHelper.asEnum<SubCategory>(parsedDataRule.DefaultDebtSubChar)).Value with | ex -> SubCategory.UNDEFINED
        if IfExists("debtChar") then rule.Category <- try (EnumHelper.asEnum<Category>(parsedDataRule.DebtChar)).Value with | ex -> Category.UNDEFINED
        if IfExists("debtSubChar") then rule.Subcategory <- try (EnumHelper.asEnum<SubCategory>(parsedDataRule.DebtSubChar)).Value with | ex -> SubCategory.UNDEFINED

        if IfExists("debtCharName") then rule.SubcategoryName <- try parsedDataRule.DebtCharName with | ex -> ""
        if IfExists("debtSubCharName") then rule.CategoryName <- try parsedDataRule.DebtSubCharName with | ex -> ""                 

        if IfExists("defaultDebtRemFnType") then  rule.DefaultDebtRemFnType <- try (EnumHelper.asEnum<RemediationFunction>(parsedDataRule.DefaultDebtRemFnType)).Value with | ex -> RemediationFunction.UNDEFINED
        if IfExists("defaultDebtRemFnCoeff") then rule.DefaultDebtRemFnCoeff <- try parsedDataRule.DefaultDebtRemFnCoeff with | ex -> ""
        if IfExists("debtOverloaded") then rule.DebtOverloaded <- try parsedDataRule.DebtOverloaded with | ex -> false

        if IfExists("debtRemFnType") then 
            rule.RemediationFunction <- try (EnumHelper.asEnum<RemediationFunction>(parsedDataRule.DebtRemFnType)).Value with | ex -> RemediationFunction.UNDEFINED

        if IfExists("debtRemFnCoeff") then
            rule.DebtRemFnCoeff <- parsedDataRule.DebtRemFnCoeff
            let value = Regex.Replace(rule.DebtRemFnCoeff, "[^0-9.]", "")
            let unit = rule.DebtRemFnCoeff.Replace(value, "").Replace("min", "MN").Replace("hour", "H").Replace("day", "D")
            rule.RemediationFactorVal <- Int32.Parse(value)
            rule.RemediationFactorTxt <- try (EnumHelper.asEnum<RemediationUnit>(unit)).Value with | ex -> RemediationUnit.UNDEFINED
        else
            rule.DebtRemFnCoeff <- ""
            rule.RemediationFactorVal <- 0
            rule.RemediationFactorTxt <- RemediationUnit.UNDEFINED

        if IfExists("params") && parsedDataRule.Params.Length > 0 && profile <> null then
            let ruleShow = 
                try
                    let url = sprintf "/api/rules/show?key=%s&actives=true" (HttpUtility.UrlEncode(rule.Repo + ":" + rule.Key))
                    let reply = httpconnector.HttpSonarGetRequest(userconf, url)
                    JsonarRuleShowResponse.Parse(reply)
                with
                | ex -> System.Diagnostics.Debug.WriteLine("Error Show rule: " + ex.Message)
                        JsonarRuleShowResponse.GetSample()

            if IfExists("templateKey") then
                rule.TemplateKey <- parsedDataRule.TemplateKey.Value.Replace(rule.Repo + ":", "")
            
            let AddParam(param:JsonRuleSearchResponse.Param) =
                try
                    let IfExistsParam(propertyToSet:string) =
                        match param.JsonValue.TryGetProperty(propertyToSet) with
                        | NotNull ->
                            true
                        | _ -> 
                            false
                    let ruleparam  = new RuleParam()
                    ruleparam.Key <- param.Key
                    ruleparam.DefaultValue <- param.DefaultValue
                    ruleparam.Value <- param.DefaultValue
                    ruleparam.Type <- param.Type
                    if IfExistsParam("htmlDesc") then
                        ruleparam.Desc <- param.HtmlDesc

                    let isFound = (List.ofSeq ruleShow.Actives) |> List.tryFind (fun active -> active.QProfile.Equals(profile.Key))
                    match isFound with
                    | Some elem -> 
                        rule.Severity <- try (EnumHelper.asEnum<Severity>(elem.Severity)).Value with | ex -> Severity.UNDEFINED
                        let paramshow = elem.Params |> Seq.tryFind (fun param -> param.Key.Equals(ruleparam.Key))
                        match paramshow with
                        | Some data -> ruleparam.Value <- data.Value.String.Value
                        | _ -> ()
                    | _ -> ()

                    rule.Params.Add(ruleparam)
                with
                | ex -> ()

            parsedDataRule.Params |> Seq.iter (fun param -> AddParam(param))



    let CreateRuleInProfile(parsedDataRule:JsonRuleSearchResponse.Rule, profile : Profile, enabledStatus:bool) =

        let IfExists(propertyToSet:string) =
            match parsedDataRule.JsonValue.TryGetProperty(propertyToSet) with
            | NotNull ->
                true
            | _ -> 
                false


        let newRule = new Rule()
        if IfExists("key") then
            newRule.Enabled <- enabledStatus
            newRule.Key <-  try parsedDataRule.Key with | ex -> ""
            newRule.ConfigKey <-  try parsedDataRule.Key with | ex -> ""

        if IfExists("repo") then newRule.Repo <- parsedDataRule.Repo
        if IfExists("name") then newRule.Name <- parsedDataRule.Name
        if IfExists("createdAt") then newRule.CreatedAt <- DateTime.Parse(parsedDataRule.CreatedAt)
        if IfExists("severity") then newRule.Severity <- try (EnumHelper.asEnum<Severity>(parsedDataRule.Severity)).Value with | ex -> Severity.UNDEFINED
        if IfExists("status") then newRule.Status <- try (EnumHelper.asEnum<Status>(parsedDataRule.Status)).Value with | ex -> Status.UNDEFINED
        if IfExists("internalKey") then newRule.InternalKey <- parsedDataRule.InternalKey

        if IfExists("isTemplate") then newRule.IsTemplate <- try parsedDataRule.IsTemplate with | ex -> false

        if IfExists("tags") then
            for tag in parsedDataRule.Tags do
                newRule.Tags.Add(tag)
        if IfExists("sysTags") then
            for tag in parsedDataRule.SysTags do
                newRule.SysTags.Add(tag)

        if IfExists("lang") then newRule.Lang <- parsedDataRule.Lang
        if IfExists("langName") then newRule.LangName <- parsedDataRule.LangName
        if IfExists("htmlDesc") then newRule.HtmlDescription <- parsedDataRule.HtmlDesc
        if IfExists("defaultDebtChar") then newRule.DefaultDebtChar <- try (EnumHelper.asEnum<Category>(parsedDataRule.DefaultDebtChar)).Value with | ex -> Category.UNDEFINED
        if IfExists("defaultDebtSubChar") then newRule.DefaultDebtSubChar <- try (EnumHelper.asEnum<SubCategory>(parsedDataRule.DefaultDebtSubChar)).Value with | ex -> SubCategory.UNDEFINED
        if IfExists("debtChar") then newRule.Category <- try (EnumHelper.asEnum<Category>(parsedDataRule.DebtChar)).Value with | ex -> Category.UNDEFINED
        if IfExists("debtSubChar") then newRule.Subcategory <- try (EnumHelper.asEnum<SubCategory>(parsedDataRule.DebtSubChar)).Value with | ex -> SubCategory.UNDEFINED

        if IfExists("debtCharName") then newRule.SubcategoryName <- try parsedDataRule.DebtCharName with | ex -> ""
        if IfExists("debtSubCharName") then newRule.CategoryName <- try parsedDataRule.DebtSubCharName with | ex -> ""                 

        if IfExists("defaultDebtRemFnType") then  newRule.DefaultDebtRemFnType <- try (EnumHelper.asEnum<RemediationFunction>(parsedDataRule.DefaultDebtRemFnType)).Value with | ex -> RemediationFunction.UNDEFINED
        if IfExists("defaultDebtRemFnCoeff") then newRule.DefaultDebtRemFnCoeff <- try parsedDataRule.DefaultDebtRemFnCoeff with | ex -> ""
        if IfExists("debtOverloaded") then newRule.DebtOverloaded <- try parsedDataRule.DebtOverloaded with | ex -> false



        if IfExists("debtRemFnType") then 
            newRule.RemediationFunction <- try (EnumHelper.asEnum<RemediationFunction>(parsedDataRule.DebtRemFnType)).Value with | ex -> RemediationFunction.UNDEFINED

        if IfExists("debtRemFnCoeff") then
            newRule.DebtRemFnCoeff <- parsedDataRule.DebtRemFnCoeff
            let value = Regex.Replace(newRule.DebtRemFnCoeff, "[^0-9.]", "")
            let unit = newRule.DebtRemFnCoeff.Replace(value, "").Replace("min", "MN").Replace("hour", "H").Replace("day", "D")
            newRule.RemediationFactorVal <- Int32.Parse(value)
            newRule.RemediationFactorTxt <- try (EnumHelper.asEnum<RemediationUnit>(unit)).Value with | ex -> RemediationUnit.UNDEFINED
        else
            newRule.DebtRemFnCoeff <- ""
            newRule.RemediationFactorVal <- 0
            newRule.RemediationFactorTxt <- RemediationUnit.UNDEFINED

        if IfExists("params") then
            try
                for param in parsedDataRule.Params do
                    let ruleparam  = new RuleParam()
                    ruleparam.Key <- param.Key
                    let isFound = (List.ofSeq newRule.Params) |> List.tryFind (fun c -> c.Key.Equals(param.Key))
                    match isFound with
                    | Some elem -> ()
                    | _ -> newRule.Params.Add(ruleparam)
                    
            with
            | ex -> ()

        profile.AddRule(newRule)  

    let GetRulesFromSearchQuery(rules : JsonRuleSearchResponse.Rule [], profile : Profile, enabledStatus:bool) =
        for parsedDataRule in rules do
            CreateRuleInProfile(parsedDataRule, profile, enabledStatus)

    interface ISonarRestService with
        // ================================
        // User Services
        // ================================
        member this.UpdateUserData(newConf : ISonarConfiguration, data:System.Collections.Generic.Dictionary<string, string>) =
            async {
                return UsersService.UpdateUserData(newConf, httpconnector, data)
            } |> Async.StartAsTask
        member this.UpdateUserLogin(newConf : ISonarConfiguration, oldLogin:string, newLogin:string) =
            async {
                return UsersService.UpdateUserLogin(newConf, httpconnector, oldLogin, newLogin)
            } |> Async.StartAsTask
        member this.UpdateIdentityProvider(newConf : ISonarConfiguration, data:System.Collections.Generic.Dictionary<string, string>) =
            async {
                return UsersService.UpdateIdentityProvider(newConf, httpconnector, data)
            } |> Async.StartAsTask

        member this.GetUserList(newConf : ISonarConfiguration) =
            async {
                return UsersService.GetUserList(newConf, httpconnector)
            } |> Async.StartAsTask
    
        member this.GetTeamsFile(teamsFile:string) =
            async {
                return UsersService.GetTeamsFile(teamsFile)
            } |> Async.StartAsTask

        member this.GetTeams(users : System.Collections.Generic.IEnumerable<User>, teamsFile:string) =
            async {
                return UsersService.GetTeams(users, teamsFile)
            } |> Async.StartAsTask

        member this.AuthenticateUser(newConf : ISonarConfiguration) =
            async {
                return UsersService.AuthenticateUser(newConf, httpconnector)
            } |> Async.StartAsTask

        // ================================
        // Project Analysis Service Calls
        // ================================
        member this.CreateVersion(conf: ISonarConfiguration, project: Resource, version: string, date : DateTime, toke:CancellationToken, logger:IRestLogger) =
            async {
                let id, date, error = AnalysisService.GetAnalysisId(conf, project, date, httpconnector)
                if id <> null then
                    return AnalysisService.CreateVersion(conf, id, version, httpconnector)
                else
                    return error
            } |> Async.StartAsTask

        member this.GetCoverageReportOnNewCodeOnLeak(conf: ISonarConfiguration, project: Resource, token:CancellationToken, logger:IRestLogger) =
            async {
                return DifferencialService.GetCoverageReportOnNewCodeOnLeak(conf, project, httpconnector, token, logger)
            } |> Async.StartAsTask
            

        member this.GetSummaryProjectReport(conf: ISonarConfiguration, project: Resource, token:CancellationToken, logger:IRestLogger) =
            async {
                return DifferencialService.GetSummaryProjectReport(conf, project, httpconnector)
            } |> Async.StartAsTask

        member this.GetCoverageReport(conf: ISonarConfiguration, project: Resource, token:CancellationToken, logger:IRestLogger) =
            async {
                return DifferencialService.GetCoverageReport(conf, project, httpconnector)
            } |> Async.StartAsTask
           
        // ======================
        // Settings Service Calls
        // ======================
        member this.IgnoreAllFile(conf: ISonarConfiguration, project: Resource, file: string) =
            SettingsService.IgnoreAllFile(conf, project, file, (this :> ISonarRestService))

        member this.IgnoreRuleOnFile(conf: ISonarConfiguration, project: Resource, file: string, rule: Rule) =
            SettingsService.IgnoreRuleOnFile(conf, project, file, rule, (this :> ISonarRestService), httpconnector)

        member this.GetExclusions(conf: ISonarConfiguration, project: Resource) =
            SettingsService.GetExclusions(conf, project, (this :> ISonarRestService))

        member this.UpdateProperty(newConf : ISonarConfiguration, id: string, value: string, projectIn: Resource) =
            SettingsService.UpdateProperty(newConf, id, value, projectIn, httpconnector)

        member this.SetSetting(newConf : ISonarConfiguration, setting:Setting, project : Resource) =
            SettingsService.SetSetting(newConf, setting, project, httpconnector)

        member this.GetSettings(newConf : ISonarConfiguration, project : Resource) =
            SettingsService.GetSettings(newConf, project, httpconnector).AsEnumerable()

        member this.CancelRequest() =
            cancelRequest <- true

        member this.IndexServerResources(conf : ISonarConfiguration, project : Resource, toke:CancellationToken, logger:IRestLogger) = 
            async {
                if conf.SonarVersion < 6.3 then
                    let url = "/api/resources/index?qualifiers=DIR,TRK,BRC&depth=-1&resource=" + project.Key
                    return getResourcesFromResponseContent(httpconnector.HttpSonarGetRequest(conf, url))
                else
                    return ComponentService.IndexServerResources(conf, project, httpconnector)
            } |> Async.StartAsTask

        member this.GetBlameLine(conf:ISonarConfiguration, key:string, line:int, toke:CancellationToken, logger:IRestLogger) = 
            async {
                let url = "/api/sources/scm?key=" + key.Trim() + "&commits_by_line=true&from=" + line.ToString() + "&to=" + line.ToString()
                let reply = httpconnector.HttpSonarGetRequest(conf, url)
                let data = ScmAnswer.Parse(reply)
                let blame = new BlameLine()
                let scmLine = data.Scm.[0]
                blame.Author <- scmLine.Strings.[1]
                blame.Date <- scmLine.DateTime.DateTime
                blame.Email <- scmLine.Strings.[1]
                blame.Line <- scmLine.Number

                return blame
            } |> Async.StartAsTask


        member this.ApplyPermissionTemplateToProject(conf:ISonarConfiguration, key:string, name:string) =
            let url = "/api/permissions/search_templates?q=" + name
            try
                let reply = httpconnector.HttpSonarGetRequest(conf, url)
                let data = TemplateSearchAnswer.Parse(reply)

                let template = data.PermissionTemplates |> Seq.find (fun c -> c.Name.Equals(name))
                let url = "/api/permissions/apply_template?projectKey=" + key.Trim() + "&templateId=" + template.Id
                let response = httpconnector.HttpSonarPostRequest(conf, url, Map.empty)
                if response.StatusCode <> Net.HttpStatusCode.NoContent then
                    "Failed to apply template id, please report issue: " + response.StatusCode.ToString() + " : " + response.Content 
                else
                    ""
            with
            | ex -> "Unable to apply or find template : " + name + " " + ex.Message                    
         
        member this.ProvisionProject(conf:ISonarConfiguration, key:string, name:string, branch:string) =
            let branchtogo = 
                if String.IsNullOrEmpty(branch.Trim()) then
                    ""
                else
                    "&branch=" + branch.Trim()

            let nametogo = 
                "&name=" + HttpUtility.UrlEncode(name.Trim())

            let keytogo =
                "?key=" + key.Trim()

            let url = "/api/projects/create" + keytogo + nametogo + branchtogo
            let response = httpconnector.HttpSonarPostRequest(conf, url, Map.empty)
            if response.StatusCode <> Net.HttpStatusCode.OK then
                "Failed to provision project: " + response.StatusCode.ToString() + " : " + response.Content 
            else
                ""
        
        member this.DeleteProfile(conf:ISonarConfiguration, profileKey : string) =
            let url = "/api/qualityprofiles/delete?profileKey=" + profileKey
            let response = httpconnector.HttpSonarPostRequest(conf, url, Map.empty)
            if response.StatusCode <> Net.HttpStatusCode.OK then
                "Failed delete profile: " + response.StatusCode.ToString()
            else
                ""

        member this.GetParentProfile(conf:ISonarConfiguration, profileKey : string) = 
            let url = "/api/qualityprofiles/inheritance?profileKey=" + profileKey
            let reply = httpconnector.HttpSonarGetRequest(conf, url)
            let data = JsonarProfileInheritance.Parse(reply)
            try
                data.Profile.Parent
            with
            | _ -> ""
                            
        member this.GetInstalledPlugins(conf:ISonarConfiguration) =
            let dicret = new System.Collections.Generic.Dictionary<string, string>()
            let url = "/api/plugins/installed"

            try
                let reply = httpconnector.HttpSonarGetRequest(conf, url)
                let data = PluginsMessage.Parse(reply)

                for plugin in data.Plugins do
                    dicret.Add(plugin.Name, plugin.Version.String.Value)

            with
            |_ -> ()

            dicret

        member this.AssignProfileToProject(conf:ISonarConfiguration, profileKey:string, projectKey:string) = 
            let url = "/api/qualityprofiles/add_project"
            let options = Map.empty.Add("profileKey", profileKey).Add("projectKey", projectKey)
            let response = httpconnector.HttpSonarPostRequest(conf, url, options)
            if response.StatusCode <> Net.HttpStatusCode.NoContent then
                "Failed change parent: " + response.StatusCode.ToString()
            else
                ""


        member this.ChangeParentProfile(conf:ISonarConfiguration, profileKey:string, parentKey:string) = 
            let url = "/api/qualityprofiles/change_parent?profileKey=" + profileKey + "&parentKey=" + parentKey
            let response = httpconnector.HttpSonarPostRequest(conf, url, Map.empty)
            if response.StatusCode <> Net.HttpStatusCode.OK then
                "Failed change parent: " + response.StatusCode.ToString()
            else
                ""

        member this.CopyProfile(conf:ISonarConfiguration, id:string, newName:string) = 
            let url = "/api/qualityprofiles/copy?fromKey=" + id + "&toName=" + HttpUtility.UrlEncode(newName)
            let response = httpconnector.HttpSonarPostRequest(conf, url, Map.empty)
            if response.StatusCode <> Net.HttpStatusCode.OK then
                ""
            else
                let data = CopyProfileAnswer.Parse(response.Content)
                let profileKey = data.Key
                profileKey

        member this.CreateRule(conf:ISonarConfiguration, rule:Rule, ruleTemplate:Rule) =
            let url ="/api/rules/create"
            let severity = if rule.Severity <> Severity.UNDEFINED then rule.Severity else Severity.MINOR
            let typeofRule = if rule.Type <> "" then rule.Type else "CODE_SMELL"

            let optionalProps =
                if conf.SonarVersion < 5.1 then
                    Map.empty.Add("custom_key", rule.Key.Replace( rule.Repo + ":", ""))
                                             .Add("html_description", rule.HtmlDescription)
                                             .Add("name", rule.Name)
                                             .Add("severity", severity.ToString())
                                             .Add("template_key", ruleTemplate.Key)
                elif conf.SonarVersion < 6.7 then
                    Map.empty.Add("custom_key", rule.Key.Replace( rule.Repo + ":", ""))
                                             .Add("html_description", rule.HtmlDescription)
                                             .Add("markdown_description", rule.MarkDownDescription)
                                             .Add("name", rule.Name)
                                             .Add("severity", severity.ToString())
                                             .Add("template_key", ruleTemplate.Key)
                else
                    Map.empty.Add("custom_key", rule.Key.Replace( rule.Repo + ":", ""))
                                             .Add("html_description", rule.HtmlDescription)
                                             .Add("markdown_description", rule.MarkDownDescription)
                                             .Add("name", rule.Name)
                                             .Add("severity", severity.ToString())
                                             .Add("template_key", ruleTemplate.Key)
                                             .Add("type", typeofRule)
                                                                 
            let errorMessages = new System.Collections.Generic.List<string>()
            let response = httpconnector.HttpSonarPostRequest(conf, url, optionalProps)
            if response.StatusCode <> Net.HttpStatusCode.OK then
                let errors = JsonErrorMessage.Parse(response.Content)
                for error in errors.Errors do
                    errorMessages.Add(error.Msg)
            
            errorMessages

        member this.GetTemplateRules(conf:ISonarConfiguration, profile:Profile) =
            let rules = new System.Collections.Generic.List<Rule>()
            let url ="/api/rules/search?is_template=true&qprofile=" + HttpUtility.UrlEncode(profile.Key) + "&languages=" + HttpUtility.UrlEncode(profile.Language)
            let reply = httpconnector.HttpSonarGetRequest(conf, url)                        
            let rules = JsonRuleSearchResponse.Parse(reply)

            GetRulesFromSearchQuery(rules.Rules, profile, true)

                        
        member this.ActivateRule(conf:ISonarConfiguration, ruleKey:string, severity:string, profilekey:string) =
            let errorMessages = new System.Collections.Generic.List<string>()
            let url ="/api/qualityprofiles/activate_rule"

            let optionalProps = Map.empty.Add("profile_key", HttpUtility.UrlEncode(profilekey))
                                         .Add("rule_key", ruleKey)
                                         .Add("severity", severity.ToString())

            let errorMessages = new System.Collections.Generic.List<string>()
            let response = httpconnector.HttpSonarPostRequest(conf, url, optionalProps)
            if response.StatusCode <> Net.HttpStatusCode.OK then
                response.Content
            else
                ""

        member this.DeleteRule(conf:ISonarConfiguration, rule:Rule) =
            let errorMessages = new System.Collections.Generic.List<string>()
            let url ="/api/rules/delete"

            let optionalProps = Map.empty.Add("key", rule.Key)

            let errorMessages = new System.Collections.Generic.List<string>()
            let response = httpconnector.HttpSonarPostRequest(conf, url, optionalProps)
            if response.StatusCode <> Net.HttpStatusCode.OK then
                let errors = JsonErrorMessage.Parse(response.Content)
                for error in errors.Errors do
                    errorMessages.Add(error.Msg)
            
            errorMessages

        member this.DisableRule(conf:ISonarConfiguration, rule:Rule, profilekey:string) =
            let errorMessages = new System.Collections.Generic.List<string>()
            let url ="/api/qualityprofiles/deactivate_rule"

            let optionalProps = Map.empty.Add("profile_key", HttpUtility.UrlEncode(profilekey))
                                         .Add("rule_key", rule.Key)

            let errorMessages = new System.Collections.Generic.List<string>()
            let response = httpconnector.HttpSonarPostRequest(conf, url, optionalProps)
            if response.StatusCode <> Net.HttpStatusCode.OK then
                let errors = JsonErrorMessage.Parse(response.Content)
                for error in errors.Errors do
                    errorMessages.Add(error.Msg)
            
            errorMessages
            
        member this.UpdateTags(conf:ISonarConfiguration, rule:Rule, tags:System.Collections.Generic.List<string>) =
            let errorMessages = new System.Collections.Generic.List<string>()
                    
            let settags = new System.Collections.Generic.HashSet<string>()
            let dic = new System.Collections.Generic.Dictionary<string, string>()
            let mutable newtags = ""
            for tag in tags do
                if not(settags.Contains(tag)) then
                    newtags <- newtags + tag + ","
                    settags.Add(tag) |> ignore

            newtags <- newtags.Trim(',')
            dic.Add("tags", newtags)

            (this :> ISonarRestService).UpdateRule(conf, rule.Key, dic)

        member this.GetAllTags(conf:ISonarConfiguration) =
            let tags = System.Collections.Generic.List<string>()
            let url ="/api/rules/tags"
            let response = httpconnector.HttpSonarGetRequest(conf, url)
            let tagsP = JsonTags.Parse(response)

            for tag in tagsP.Tags do
                tags.Add(tag)

            tags

        member this.UpdateRule(conf:ISonarConfiguration, key:string, optionalProps:System.Collections.Generic.Dictionary<string, string>) = 
            let url ="/api/rules/update?key=" + HttpUtility.UrlEncode(key)
            let errorMessages = new System.Collections.Generic.List<string>()
            let response = httpconnector.HttpSonarPostRequestDic(conf, url, optionalProps)
            if response.StatusCode <> Net.HttpStatusCode.OK then
                let errors = JsonErrorMessage.Parse(response.Content)
                for error in errors.Errors do
                    errorMessages.Add(error.Msg)
            
            errorMessages

        member this.UpdateRuleData(conf:ISonarConfiguration, newRule : Rule) = 
            let url = 
                if newRule.Key.StartsWith(newRule.Repo + ":") then
                    "/api/rules/search?rule_key=" + HttpUtility.UrlEncode(newRule.Key) + "&facets=types"
                else
                    "/api/rules/search?rule_key=" + HttpUtility.UrlEncode(newRule.Repo) + ":" + HttpUtility.UrlEncode(newRule.Key) + "&facets=types"
            try
                System.Diagnostics.Debug.WriteLine(url)
                let reply = httpconnector.HttpSonarGetRequest(conf, url)
                let rules = JsonRuleSearchResponse.Parse(reply)
                if rules.Total = 1 then
                    newRule.DefaultDebtRemFnType <- RemediationFunction.UNDEFINED
                    newRule.DefaultDebtChar <- Category.UNDEFINED
                    newRule.DefaultDebtSubChar <- SubCategory.UNDEFINED
                    // update values except for severity, since this is the default severity
                    UpdateRuleInProfile(rules.Rules.[0], newRule, true, conf, null)

                    try
                        for facet in rules.Facets do
                            if facet.Values.[0].Val <> "NONE" then
                                newRule.Subcategory <- try (EnumHelper.asEnum<SubCategory>(facet.Values.[0].Val)).Value with | ex -> SubCategory.UNDEFINED                                            
                                newRule.Category <- try (EnumHelper.asEnum<Category>(facet.Values.[1].Val)).Value with | ex -> Category.UNDEFINED                                        
                    with
                    | ex -> ()

                    newRule.IsParamsRetrivedFromServer <- true
            with
            | ex -> System.Diagnostics.Debug.WriteLine("FAILED: " + url + " : ", ex.Message)

        member this.GetRulesForProfile(conf:ISonarConfiguration, profile:Profile, searchData:bool) = 
            if profile <> null then
                if conf.SonarVersion < 5.2 then
                    let url = "/api/profiles/index?language=" + HttpUtility.UrlEncode(profile.Language) + "&name=" + HttpUtility.UrlEncode(profile.Name)
                    System.Diagnostics.Debug.WriteLine(url)
                    let reply = httpconnector.HttpSonarGetRequest(conf, url)
                    let data = JsonProfileAfter44.Parse(reply)
                    let rules = data.[0].Rules
                    for rule in rules do
                        let newRule = new Rule()
                        newRule.Key <- rule.Key
                        newRule.Repo <- rule.Repo
                        match rule.Params with
                        | NotNull ->
                            for para in rule.Params do
                                let param = new RuleParam()
                                param.Key <- para.Key
                                param.DefaultValue <- para.Value.ToString()
                                param.Value <- para.Value.ToString()
                                let isFound = (List.ofSeq newRule.Params) |> List.tryFind (fun c -> c.Key.Equals(para.Key))
                                match isFound with
                                | Some elem -> ()
                                | _ -> newRule.Params.Add(param)

                        | _ -> ()

                        newRule.Severity <- (EnumHelper.asEnum<Severity>(rule.Severity)).Value

                        if searchData then
                            let url = "/api/rules/search?rule_key=" + HttpUtility.UrlEncode(rule.Repo + ":" + rule.Key)
                            try
                                System.Diagnostics.Debug.WriteLine(url)
                                let reply = httpconnector.HttpSonarGetRequest(conf, url)
                                let rules = JsonRuleSearchResponse.Parse(reply)
                                if rules.Total = 1 then
                                    // update values except for severity, since this is the default severity
                                    UpdateRuleInProfile(rules.Rules.[0], newRule, true, conf, null)
                            with
                            | ex -> System.Diagnostics.Debug.WriteLine("FAILED: " + url + " : ", ex.Message)

                        try
                            profile.AddRule(newRule)
                        with
                        | ex -> System.Diagnostics.Debug.WriteLine("Cannot Add Rule: " + newRule.ConfigKey + " ex: " + ex.Message)
                else
                    let rec GetRules(page:int) = 
                        let url = sprintf "/api/rules/search?activation=true&qprofile=%s&p=%i" (HttpUtility.UrlEncode(profile.Key)) page
                        let reply = httpconnector.HttpSonarGetRequest(conf, url)
                        let rules = JsonRuleSearchResponse.Parse(reply)

                        let CreateRuleInProfile(rule:JsonRuleSearchResponse.Rule) =
                            let newRule = new Rule()
                            UpdateRuleInProfile(rule, newRule, false, conf, profile)
                            profile.AddRule(newRule)

                        rules.Rules |> Seq.iter (fun rule -> CreateRuleInProfile(rule))

                        if rules.Rules.Length = rules.Ps then
                            GetRules(page + 1)

                    GetRules(1)

                    
        member this.GetRuleUsingProfileAppId(conf:ISonarConfiguration, key:string) = 
                let url = "/api/rules/search?&rule_key=" + HttpUtility.UrlEncode(key)
                let reply = httpconnector.HttpSonarGetRequest(conf, url)
                let rules = JsonRuleSearchResponse.Parse(reply)
                if rules.Total = 1 then
                    let profile = new Profile(this :> ISonarRestService, conf)
                    CreateRuleInProfile(rules.Rules.[0], profile, false)
                    profile.GetRule(key)
                else
                    null

        member this.GetRulesForProfileUsingRulesApp(conf:ISonarConfiguration , profile:Profile, active:bool) = 
            if profile <> null then
                let getActivation() =
                    if active then
                        "&activation=true" 
                    else
                        "&activation=false" 

                let url = "/api/rules/search?ps=200&qprofile=" + HttpUtility.UrlEncode(profile.Key) + getActivation() + "&languages=" + HttpUtility.UrlEncode(profile.Language)
                let reply = httpconnector.HttpSonarGetRequest(conf, url)                        
                let rules = JsonRuleSearchResponse.Parse(reply)
                let numberOfPages = rules.Total / rules.Ps + 1

                GetRulesFromSearchQuery(rules.Rules, profile, active)
                

                for i = 2 to numberOfPages do
                    let url = "/api/rules/search?ps=200&qprofile=" + HttpUtility.UrlEncode(profile.Key) + getActivation() + "&p=" + Convert.ToString(i)
                    let reply = httpconnector.HttpSonarGetRequest(conf, url)
                    let rules = JsonRuleSearchResponse.Parse(reply)
                    GetRulesFromSearchQuery(rules.Rules, profile, active)
                 
                ()

        member this.GetProfilesUsingRulesApp(conf : ISonarConfiguration) = 
            let profiles = new System.Collections.Generic.List<Profile>()
            try
                let reply = httpconnector.HttpSonarGetRequest(conf, "/api/rules/app")
                let data = JsonInternalData.Parse(reply)
                for profile in data.Qualityprofiles do
                    let newprofile = new Profile(this :> ISonarRestService, conf)
                    newprofile.Key <- profile.Key
                    newprofile.Language <- profile.Lang
                    newprofile.Name <- profile.Name
                    profiles.Add(newprofile)
            with
            | ex -> ()

            profiles

        member this.GetAvailableProfiles(conf : ISonarConfiguration) = 
            
            let profiles = new System.Collections.Generic.List<Profile>()
            if conf.SonarVersion < 8.0 then
              let reply = httpconnector.HttpSonarGetRequest(conf, "/api/profiles/list")
              let data = JsonQualityProfiles.Parse(reply)
              for profile in data do
                  let newprofile = new Profile(this :> ISonarRestService, conf)
                  newprofile.Language <- profile.Language
                  newprofile.Name <- profile.Name
                  newprofile.Default <- profile.Default
                  profiles.Add(newprofile)
              profiles
            else
              let reply = httpconnector.HttpSonarGetRequest(conf, "/api/qualityprofiles/search")
              let data = JsonQualityProfiles80.Parse(reply)
              for profile in data.Profiles do
                  let newprofile = new Profile(this :> ISonarRestService, conf)
                  newprofile.Language <- profile.Language
                  newprofile.Name <- profile.Name
                  newprofile.Default <- profile.IsDefault
                  newprofile.Key <- profile.Key
                  profiles.Add(newprofile)
              profiles

        member this.GetProjects(newConf:ISonarConfiguration) = 
            let projects = new System.Collections.Generic.List<SonarProject>()
            let reply = httpconnector.HttpSonarGetRequest(newConf, "/api/projects/index")
            let serverProjects = JsonProjectIndex.Parse(reply)
            for project in serverProjects do
                let projectToAdd = new SonarProject()
                projectToAdd.Name <- project.Nm
                projectToAdd.Qualifier <- project.Qu
                projectToAdd.Scope <- project.Sc
                projectToAdd.Key <- project.K
                projectToAdd.Id <- project.Id
                projects.Add(projectToAdd)

            projects

        member this.GetIssues(newConf : ISonarConfiguration, query : string, project : string, token:CancellationToken, logger:IRestLogger) = 
            async {
                let url =  "/api/issues/search" + query + "&additionalFields=comments&pageSize=200"
                return IssuesService.SearchForIssues(newConf, url, token, httpconnector, logger)
            } |> Async.StartAsTask

        member this.GetIssuesByAssigneeInProject(newConf : ISonarConfiguration, project : string, login : string, token:CancellationToken, logger:IRestLogger) = 
            async {
                let url =  "/api/issues/search?componentRoots=" + project + "&assignees="+ login+ "&pageSize=200&statuses=OPEN,CONFIRMED,REOPENED"
                return IssuesService.SearchForIssues(newConf, url, token, httpconnector, logger)
            } |> Async.StartAsTask
   
        member this.GetAllIssuesByAssignee(newConf : ISonarConfiguration, login : string, token:CancellationToken, logger:IRestLogger) = 
            async {
                let url =  "/api/issues/search?assignees="+ login + "&pageSize=200&statuses=OPEN,CONFIRMED,REOPENED"
                return IssuesService.SearchForIssues(newConf, url, token, httpconnector, logger)
            } |> Async.StartAsTask

        member this.GetIssuesForProjectsCreatedAfterDate(newConf : ISonarConfiguration, project : string, date : DateTime, token:CancellationToken, logger:IRestLogger) =
            async {
                let url =  "/api/issues/search?componentRoots=" + project + "&pageSize=200&createdAfter=" + Convert.ToString(date.Year) + "-" + Convert.ToString(date.Month) + "-"  + Convert.ToString(date.Day) + "&statuses=OPEN,CONFIRMED,REOPENED"
                return IssuesService.SearchForIssues(newConf, url, token, httpconnector, logger)
            } |> Async.StartAsTask

        member this.GetIssuesForProjects(newConf : ISonarConfiguration, project : string, token:CancellationToken, logger:IRestLogger) =
            async {
                let url =  "/api/issues/search?componentRoots=" + project + "&pageSize=200&statuses=OPEN,CONFIRMED,REOPENED"
                return IssuesService.SearchForIssues(newConf, url, token, httpconnector, logger)
            } |> Async.StartAsTask

        member this.GetIssuesInResource(conf : ISonarConfiguration, resource : string, token:CancellationToken, logger:IRestLogger) =
            async {
                return IssuesService.SearchForIssuesInResource(conf, resource, httpconnector, logger)
            } |> Async.StartAsTask

        member this.SetIssueTags(conf : ISonarConfiguration, issue : Issue, tags : System.Collections.Generic.List<string>, token:CancellationToken, logger:IRestLogger) =
            async {
                return IssuesService.SetIssueTags(conf, issue, tags, httpconnector, token, logger)
            } |> Async.StartAsTask

        member this.GetAvailableTags(newConf : ISonarConfiguration, token:CancellationToken, logger:IRestLogger) =
            async {
                return IssuesService.GetAvailableTags(newConf, httpconnector, token, logger)
            } |> Async.StartAsTask

        member this.SearchComponent(conf : ISonarConfiguration, searchString : string, filterBranchs : bool, masterBranch : string) = 
            let url = "/api/components/search?qualifiers=DIR,TRK,PAC,CLA,BRC&q=" + searchString
            
            let getComponentFromResponse(data : string) = 
                let resources = new System.Collections.Generic.List<Resource>()
                let components = ComponentSearchAnswer.Parse(data)
                for comp in components.Components do
                    let newRes = new Resource()
                    newRes.Name <- comp.Name
                    newRes.Key <- comp.Key
                    newRes.Qualifier <- comp.Qualifier

                    if filterBranchs then
                        if comp.Name.Contains(" ") then
                            let lastElem = comp.Name.Split(' ')
                            let branchName = lastElem.[lastElem.Length - 1]
                            if not(comp.Key.EndsWith(":" + branchName)) || branchName = masterBranch then
                                resources.Add(newRes)
                        else
                            resources.Add(newRes)
                    else
                        resources.Add(newRes)

                // we need to get all pages
                let value = int(System.Math.Ceiling(float(components.Paging.Total) / float(components.Paging.PageSize)))

                for i = 2 to value do
                    if not(cancelRequest) then
                        let url = url + "&pageIndex=" + Convert.ToString(i)
                        let response = httpconnector.HttpSonarGetRequest(conf, url)
                        let components = ComponentSearchAnswer.Parse(data)
                        for comp in components.Components do
                            let newRes = new Resource()
                            newRes.Name <- comp.Name
                            newRes.Key <- comp.Key
                            newRes.Qualifier <- comp.Qualifier
                            resources.Add(newRes)
                resources

            getComponentFromResponse(httpconnector.HttpSonarGetRequest(conf, url))

        member this.GetResourcesData(conf : ISonarConfiguration, resource : string) =
            if conf.SonarVersion >= 6.3 && conf.SonarVersion < 8.0 then
                let url = "/api/components/show?key=" + resource
                let response = httpconnector.HttpSonarGetRequest(conf, url)
                let componentData = JsonComponentShow.Parse(response)
                let resource = new Resource()
                resource.IdType <- componentData.Component.Id
                resource.Key <- componentData.Component.Key
                resource.Name <- componentData.Component.Name
                let keysElements = componentData.Component.Key.Split(':')
                if componentData.Component.Name.EndsWith(" " + keysElements.[keysElements.Length - 1]) then
                    // this is brancnh
                    resource.IsBranch <- true
                    resource.BranchName <- keysElements.[keysElements.Length - 1]

                resource.Qualifier <- componentData.Component.Qualifier
                resource.Path <- componentData.Component.Path
                let resourcelist = System.Collections.Generic.List<Resource>()
                resourcelist.Add(resource)
                resourcelist
            elif conf.SonarVersion >= 8.0 then
                let url = "/api/components/show?component=" + resource
                let response = httpconnector.HttpSonarGetRequest(conf, url)
                let componentData = JsonComponentShow.Parse(response)
                let resource = new Resource()
                resource.IdType <- componentData.Component.Id
                resource.Key <- componentData.Component.Key
                resource.Name <- componentData.Component.Name
                let keysElements = componentData.Component.Key.Split(':')
                if componentData.Component.Name.EndsWith(" " + keysElements.[keysElements.Length - 1]) then
                    // this is brancnh
                    resource.IsBranch <- true
                    resource.BranchName <- keysElements.[keysElements.Length - 1]

                resource.Qualifier <- componentData.Component.Qualifier
                resource.Path <- componentData.Component.Path
                let resourcelist = System.Collections.Generic.List<Resource>()
                resourcelist.Add(resource)
                resourcelist
            else
                let url = "/api/resources?resource=" + resource
                getResourcesFromResponseContent(httpconnector.HttpSonarGetRequest(conf, url))

        member this.GetQualityProfile(conf : ISonarConfiguration, project : Resource) =
            let resource =
                if String.IsNullOrEmpty(project.BranchName) then
                    project.Key
                else
                    if not(project.Key.EndsWith(":" + project.BranchName))  then
                        project.Key + ":" + project.BranchName
                    else
                        project.Key

            let url = "/api/resources?resource=" + resource + "&metrics=profile"
            getResourcesFromResponseContent(httpconnector.HttpSonarGetRequest(conf, url))

        member this.GetQualityProfilesForProject(conf : ISonarConfiguration, project : Resource) = 
            let resource =
                if String.IsNullOrEmpty(project.BranchName) then
                    project.Key
                else
                    if not(project.Key.EndsWith(":" + project.BranchName))  then
                        project.Key + ":" + project.BranchName
                    else
                        project.Key
                    
            if conf.SonarVersion >= 5.2 then
                let url = "/api/qualityprofiles/search?projectKey=" + resource
                let response = httpconnector.HttpSonarGetRequest(conf, url)
                let profilesJson = JsonQualityProfiles63.Parse(response)
                let profiles = new System.Collections.Generic.List<Profile>()

                let AddProfile(profile:JsonQualityProfiles63.Profile) =
                    let newProfile = new Profile(this :> ISonarRestService, conf)
                    newProfile.Key <- profile.Key
                    newProfile.Name <- profile.Name
                    newProfile.Language <- profile.Language
                    profiles.Add(newProfile)

                profilesJson.Profiles |> Seq.iter (fun profile -> AddProfile(profile))

                profiles
            else
                let url =
                    if conf.SonarVersion < 8.0 then
                        "/api/profiles/list?project=" + resource
                    else
                        "/api/qualityprofiles/search?project=" + resource
               
                GetQualityProfilesFromContent(httpconnector.HttpSonarGetRequest(conf, url), conf, this :> ISonarRestService)
                        
        member this.GetQualityProfilesForProject(conf : ISonarConfiguration, project : Resource, language : string) = 
            let resource =
                if String.IsNullOrEmpty(project.BranchName) then
                    project.Key
                else
                    if not(project.Key.EndsWith(":" + project.BranchName))  then
                        project.Key + ":" + project.BranchName
                    else
                        project.Key

            let url =
                if conf.SonarVersion < 8.0 then
                    "/api/profiles/list?project=" + resource + "&language=" + HttpUtility.UrlEncode(language)
                else
                    "/api/qualityprofiles/search?project=" + resource + "&language=" + HttpUtility.UrlEncode(language)

            GetQualityProfilesFromContent(httpconnector.HttpSonarGetRequest(conf, url), conf, this :> ISonarRestService)

        member this.GetProjectsList(conf : ISonarConfiguration) = 
            if conf.SonarVersion < 6.3 then
                let url = "/api/resources"
                getResourcesFromResponseContent(httpconnector.HttpSonarGetRequest(conf, url))
            else
                ComponentService.SearchProjects(conf, httpconnector)

        member this.GetEnabledRulesInProfile(conf : ISonarConfiguration, language : string, profile : string) =
            let url = "/api/profiles?language=" + HttpUtility.UrlEncode(language) + "&name=" + HttpUtility.UrlEncode(profile)
            GetProfileFromContent(httpconnector.HttpSonarGetRequest(conf, url), conf, this :> ISonarRestService)

        member this.GetRules(conf : ISonarConfiguration, language : string) = 
            let GetLanguageUrl =
                if language = "" then
                    ""
                else
                    "?language=" + HttpUtility.UrlEncode(language)

            let url = "/api/rules" + GetLanguageUrl
            getRulesFromResponseContent(httpconnector.HttpSonarGetRequest(conf, url))

        member this.GetServerInfo(conf : ISonarConfiguration) = 
            let url = "/api/server/version"
            let responsecontent = httpconnector.HttpSonarGetRequest(conf, url)
            let versionstr = responsecontent
            let elems = versionstr.Split('.')
            float32 (elems.[0] + "." + Regex.Replace(elems.[1], @"[^\d]", ""))
            
        member this.GetSourceForFileResource(conf : ISonarConfiguration, resource : string) =
            if conf.SonarVersion < 5.0 then
                GetSourceFromContent(httpconnector.HttpSonarGetRequest(conf, "/api/sources?resource=" + resource))
            else
                GetSourceFromRaw(httpconnector.HttpSonarGetRequest(conf, "/api/sources/raw?key=" + resource))

        member this.GetCoverageInResource(conf : ISonarConfiguration, resource : string) =
            if conf.SonarVersion < 6.3 then
                let url = "/api/resources?resource=" + resource + "&metrics=coverage_line_hits_data,conditions_by_line,covered_conditions_by_line"
                GetCoverageFromContent(httpconnector.HttpSonarGetRequest(conf, url))
            else
                let url = "/api/measures/component?componentKey=" + resource + "&metricKeys=coverage_line_hits_data,conditions_by_line,covered_conditions_by_line"
                MeasuresService.GetCoverageFromMeasures(httpconnector.HttpSonarGetRequest(conf, url))

        member this.GetDuplicationsDataInResource(conf : ISonarConfiguration, resource : string) =
            if conf.SonarVersion < 6.3 then
                let url = "/api/resources?resource=" + resource + "&metrics=duplications_data";
                GetDuplicationsFromContent(httpconnector.HttpSonarGetRequest(conf, url))
            else
                let url = "/api/measures/component?componentKey=" + resource + "&metricKeys=duplications_data";
                GetDuplicationsFromContent(httpconnector.HttpSonarGetRequest(conf, url))

        member this.CommentOnIssues(newConf : ISonarConfiguration, issues : System.Collections.Generic.IEnumerable<Issue>, comment : string, logger:IRestLogger, token:CancellationToken) =
            async {
                for issue in issues  do
                    if not(token.IsCancellationRequested) then
                        let mutable idstr = Convert.ToString(issue.Key)
                        let parameters = Map.empty.Add("issue", idstr).Add("text", comment)

                        let AddCommentFromResponse(issuesToCheck : System.Collections.Generic.List<Issue>) = 
                            for commentNew in issuesToCheck.[0].Comments do
                                let mutable found = false
                                for issueExist in issue.Comments do
                                    if issueExist.HtmlText = commentNew.HtmlText then
                                        found <- true

                                if not found then
                                    issue.Comments.Add(commentNew)

                        let mutable response = httpconnector.HttpSonarPostRequest(newConf, "/api/issues/add_comment", parameters)
                        if response.StatusCode <> Net.HttpStatusCode.OK then
                            idstr <- Convert.ToString(issue.Id)
                            if issue.Comments.Count > 0 then
                                let parameters = Map.empty.Add("id", Convert.ToString(issue.Comments.[0].Id)).Add("comment", comment)
                                response <- httpconnector.HttpSonarPutRequest(newConf, "/api/reviews/add_comment", parameters)
                            else
                                let url = "/api/reviews?violation_id=" + Convert.ToString(idstr) + "&status=OPEN" + GetComment(comment)
                                response <- httpconnector.HttpSonarRequest(newConf, url, Method.Post)

                        try
                            let comment = JSonComment.Parse(response.Content)
                            let commentToAdd = new Comment()
                            commentToAdd.CreatedAt <- comment.Comment.CreatedAt.DateTime
                            commentToAdd.HtmlText <- comment.Comment.HtmlText
                            commentToAdd.Key <- comment.Comment.Key
                            commentToAdd.Login <- comment.Comment.Login
                            issue.Comments.Add(commentToAdd)
                        with
                            | ex -> AddCommentFromResponse(IssuesService.getReviewsFromString(response.Content))

                return true
            } |> Async.StartAsTask
                       
        member this.MarkIssuesAsWontFix(newConf : ISonarConfiguration, issues : System.Collections.Generic.IEnumerable<Issue>, comment : string, logger:IRestLogger, token:CancellationToken) =
            async {
                for issue in issues do
                    if not(token.IsCancellationRequested) && issue.Status <> IssueStatus.RESOLVED then
                        let mutable idstr = issue.Key.ToString()
                        let mutable status = IssuesService.DoStateTransition(newConf, issue, IssueStatus.RESOLVED, "wontfix", httpconnector)
                        if status = Net.HttpStatusCode.OK then
                            if not(String.IsNullOrEmpty(comment)) then
                                (this :> ISonarRestService).CommentOnIssues(newConf, issues, comment, logger, token)
                                |> Async.AwaitTask
                                |> ignore
                        else
                            if not(idstr.Equals("0")) then
                                idstr <- Convert.ToString(issue.Id)
                                let CheckReponse(response : RestResponse)= 
                                    if response.StatusCode = Net.HttpStatusCode.OK then
                                        let reviews = IssuesService.getReviewsFromString(response.Content)
                                        issue.Id <- reviews.[0].Id
                                        issue.Status <- reviews.[0].Status
                                        issue.Resolution <- Resolution.WONTFIX
                                        let newComment = new Comment()
                                        newComment.CreatedAt <- DateTime.Now
                                        newComment.HtmlText <- comment
                                        newComment.Login <- newConf.Username
                                        issue.Comments.Add(newComment)

                                if issue.Comments.Count > 0 then
                                    let parameters = Map.empty.Add("id", idstr).Add("resolution", "WONTFIX").Add("comment", comment)
                                    let response = httpconnector.HttpSonarPutRequest(newConf, "/api/reviews/resolve", parameters)
                                    status <- response.StatusCode
                                    CheckReponse(response)
                                else
                                    let url = "/api/reviews?violation_id=" + Convert.ToString(idstr) + "&status=RESOLVED&resolution=WONTFIX" + GetComment(comment)
                                    let response = httpconnector.HttpSonarRequest(newConf, url, Method.Post)
                                    CheckReponse(response)
                                    status <- response.StatusCode
                return true
            } |> Async.StartAsTask


        member this.MarkIssuesAsFalsePositive(newConf : ISonarConfiguration, issues : System.Collections.Generic.IEnumerable<Issue>, comment : string, logger:IRestLogger, token:CancellationToken) =
            async  {
                for issue in issues do
                    if not(token.IsCancellationRequested) && issue.Status <> IssueStatus.RESOLVED then
                        let mutable idstr = issue.Key.ToString()
                        let mutable status = IssuesService.DoStateTransition(newConf, issue, IssueStatus.RESOLVED, "falsepositive", httpconnector)
                        if status = Net.HttpStatusCode.OK then
                            if not(String.IsNullOrEmpty(comment)) then
                                (this :> ISonarRestService).CommentOnIssues(newConf, issues, comment, logger, token)
                                |> Async.AwaitTask
                                |> ignore
                        else
                            if not(idstr.Equals("0")) then
                                idstr <- Convert.ToString(issue.Id)
                                let CheckReponse(response : RestResponse)= 
                                    if response.StatusCode = Net.HttpStatusCode.OK then
                                        let reviews = IssuesService.getReviewsFromString(response.Content)
                                        issue.Id <- reviews.[0].Id
                                        issue.Status <- reviews.[0].Status
                                        issue.Resolution <- Resolution.FALSE_POSITIVE
                                        let newComment = new Comment()
                                        newComment.CreatedAt <- DateTime.Now
                                        newComment.HtmlText <- comment
                                        newComment.Login <- newConf.Username
                                        issue.Comments.Add(newComment)

                                if issue.Comments.Count > 0 then
                                    let parameters = Map.empty.Add("id", idstr).Add("resolution", "FALSE-POSITIVE").Add("comment", comment)
                                    let response = httpconnector.HttpSonarPutRequest(newConf, "/api/reviews/resolve", parameters)
                                    status <- response.StatusCode
                                    CheckReponse(response)
                                else
                                    let url = "/api/reviews?violation_id=" + Convert.ToString(idstr) + "&status=RESOLVED&resolution=FALSE-POSITIVE" + GetComment(comment)
                                    let response = httpconnector.HttpSonarRequest(newConf, url, Method.Post)
                                    CheckReponse(response)
                                    status <- response.StatusCode
                return true
            } |> Async.StartAsTask

        member this.PlanIssues(newConf : ISonarConfiguration, issues : System.Collections.Generic.IEnumerable<Issue>, planid : string, logger:IRestLogger, token:CancellationToken) =
            async  {
                for issue in issues do
                    if not(token.IsCancellationRequested) then
                        let parameters = Map.empty.Add("issue", issue.Key.ToString()).Add("plan", planid)
                        let data = httpconnector.HttpSonarPostRequest(newConf, "/api/issues/plan", parameters)
                        logger.ReportMessage(sprintf "Issue Planned: %s: %A" (issue.Key.ToString()) data.StatusCode)
                return true
            } |> Async.StartAsTask


        member this.UnPlanIssues(newConf : ISonarConfiguration, issues : System.Collections.Generic.IEnumerable<Issue>, logger:IRestLogger, token:CancellationToken) =
            async  {
                for issue in issues do
                    if not(token.IsCancellationRequested) then
                        let parameters = Map.empty.Add("issue", issue.Key.ToString())
                        let data = httpconnector.HttpSonarPostRequest(newConf, "/api/issues/plan", parameters)
                        logger.ReportMessage(sprintf "Issue Planned: %s: %A" (issue.Key.ToString()) data.StatusCode)
                return true
            } |> Async.StartAsTask
                        
        member this.ResolveIssues(newConf : ISonarConfiguration, issues : System.Collections.Generic.IEnumerable<Issue>, comment : string, logger:IRestLogger, token:CancellationToken) =
            async  {
                let ProcessIssue(issue:Issue) =
                    let mutable idstr = issue.Key.ToString()
                    let mutable status = Net.HttpStatusCode.OK
                    status <- IssuesService.DoStateTransition(newConf, issue, IssueStatus.RESOLVED, "resolve", httpconnector)

                    if status = Net.HttpStatusCode.OK then
                        if not(String.IsNullOrEmpty(comment)) then
                            (this :> ISonarRestService).CommentOnIssues(newConf, issues, comment, logger, token)
                            |> Async.AwaitTask
                            |> ignore
                    else
                        idstr <- Convert.ToString(issue.Id)
                        let CheckReponse(response : RestResponse)= 
                            if response.StatusCode = Net.HttpStatusCode.OK then
                                let reviews = IssuesService.getReviewsFromString(response.Content)
                                issue.Id <- reviews.[0].Id
                                issue.Status <- reviews.[0].Status
                                issue.Resolution <- Resolution.FIXED
                                let newComment = new Comment()
                                newComment.CreatedAt <- DateTime.Now
                                newComment.HtmlText <- comment
                                newComment.Login <- newConf.Username
                                issue.Comments.Add(newComment)

                        if issue.Comments.Count > 0 then
                            let parameters = Map.empty.Add("id", idstr).Add("resolution", "FIXED").Add("comment", comment)
                            let response = httpconnector.HttpSonarPutRequest(newConf, "/api/reviews/resolve", parameters)
                            status <- response.StatusCode
                            CheckReponse(response)
                        else
                            let url = "/api/reviews?violation_id=" + Convert.ToString(idstr) + "&status=RESOLVED&resolution=FIXED" + GetComment(comment)
                            let response = httpconnector.HttpSonarRequest(newConf, url, Method.Post)
                            CheckReponse(response)
                            status <- response.StatusCode

                        logger.ReportMessage(sprintf "Issue Resolved Status: %s: %A" (issue.Key.ToString()) status)
                issues
                |> Seq.iter (fun elem -> if not(token.IsCancellationRequested) && elem.Status <> IssueStatus.RESOLVED then ProcessIssue(elem))

                return true
            } |> Async.StartAsTask

        member this.ReOpenIssues(newConf : ISonarConfiguration, issues : System.Collections.Generic.List<Issue>, comment : string, logger:IRestLogger, token:CancellationToken) =
            async {
                for issue in issues do
                    if not(token.IsCancellationRequested) && (issue.Status <> IssueStatus.REOPENED || issue.Status <> IssueStatus.OPEN) then
                        let mutable idstr = issue.Key.ToString()
                        let mutable status = Net.HttpStatusCode.OK
                        status <- IssuesService.DoStateTransition(newConf, issue, IssueStatus.REOPENED, "reopen", httpconnector)
                        if status = Net.HttpStatusCode.OK then
                            if not(String.IsNullOrEmpty(comment)) then
                                (this :> ISonarRestService).CommentOnIssues(newConf, issues, comment, logger, token)
                                |> Async.AwaitTask
                                |> ignore
                        else
                            idstr <- Convert.ToString(issue.Id)
                            let CheckReponse(response : RestResponse)= 
                                if response.StatusCode = Net.HttpStatusCode.OK then
                                    let reviews = IssuesService.getReviewsFromString(response.Content)
                                    issue.Id <- reviews.[0].Id
                                    issue.Status <- reviews.[0].Status
                                    let newComment = new Comment()
                                    newComment.CreatedAt <- DateTime.Now
                                    newComment.HtmlText <- comment
                                    newComment.Login <- newConf.Username
                                    issue.Comments.Add(newComment)

                            let parameters = Map.empty.Add("id", idstr).Add("comment", comment)
                            let response = httpconnector.HttpSonarPutRequest(newConf, "/api/reviews/reopen", parameters)
                            CheckReponse(response)
                            status <- response.StatusCode
                        
                return true
            } |> Async.StartAsTask

        member this.UnConfirmIssues(newConf : ISonarConfiguration, issues : System.Collections.Generic.IEnumerable<Issue>, comment : string, logger:IRestLogger, token:CancellationToken) =
            async {
                for issue in issues do
                    if not(token.IsCancellationRequested) then
                        let status = IssuesService.DoStateTransition(newConf, issue, IssueStatus.REOPENED, "unconfirm", httpconnector)
                        logger.ReportMessage(sprintf "Issue Unconfirmed Status: %s: %A" (issue.Key.ToString()) status)

                if not(String.IsNullOrEmpty(comment)) then
                    (this :> ISonarRestService).CommentOnIssues(newConf, issues, comment, logger, token)
                    |> Async.AwaitTask
                    |> ignore
                return true
            } |> Async.StartAsTask


        member this.ConfirmIssues(newConf : ISonarConfiguration, issues : System.Collections.Generic.IEnumerable<Issue>, comment : string, logger:IRestLogger, token:CancellationToken) =
            async {
                for issue in issues do
                    if not(token.IsCancellationRequested) && issue.Status.Equals(IssueStatus.RESOLVED) then
                        let status = IssuesService.DoStateTransition(newConf, issue, IssueStatus.REOPENED, "reopen", httpconnector) |> ignore
                        logger.ReportMessage(sprintf "Issue Confirm Status: %s: %A" (issue.Key.ToString()) status)

                if not(String.IsNullOrEmpty(comment)) then
                    (this :> ISonarRestService).CommentOnIssues(newConf, issues, comment, logger, token)
                    |> Async.AwaitTask
                    |> ignore
                
                return true
            } |> Async.StartAsTask

        member this.AssignIssuesToUser(newConf : ISonarConfiguration, issues : System.Collections.Generic.IEnumerable<Issue>, user : User, comment : string, logger:IRestLogger, token:CancellationToken) =
            async {
                for issue in issues do
                    if not(String.IsNullOrEmpty(user.Login)) then
                        let parameters = Map.empty.Add("issue", issue.Key.ToString()).Add("assignee", user.Login)
                        let data = httpconnector.HttpSonarPostRequest(newConf, "/api/issues/assign", parameters)
                        logger.ReportMessage(sprintf "Assign Issue: %s: %A" (issue.Key.ToString()) data.StatusCode)
                        if data.StatusCode = Net.HttpStatusCode.OK then
                            issue.Assignee <- user.Login
                    else
                        let parameters = Map.empty.Add("issue", issue.Key.ToString())
                        let data = httpconnector.HttpSonarPostRequest(newConf, "/api/issues/assign", parameters)
                        if data.StatusCode = Net.HttpStatusCode.OK then
                            issue.Assignee <- ""
                        logger.ReportMessage(sprintf "Assign Issue: %s: %A" (issue.Key.ToString()) data.StatusCode)
                
                if not(String.IsNullOrEmpty(comment)) then
                    (this :> ISonarRestService).CommentOnIssues(newConf, issues, comment, logger, token)
                    |> Async.AwaitTask
                    |> ignore

                return true
            } |> Async.StartAsTask
