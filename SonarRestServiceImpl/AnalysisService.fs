﻿module AnalysisService

open FSharp.Data
open FSharp.Data.JsonExtensions
open SonarRestService
open SonarRestService.Types
open SonarRestServiceImpl
open System

type ProjectAnalysis = JsonProvider<""" {"paging":{"pageIndex":1,"pageSize":100,"total":89},"analyses":[{"key":"AVs_7-vgJwp3Pa-Sdbwx","date":"2017-04-06T00:04:57+0300","events":[{"key":"AVs_7_2_Jwp3Pa-Sdbwy","category":"VERSION","name":"work"}]},{"key":"AVs61HTHl-sCHwhjXyzy","date":"2017-04-05T00:14:04+0300","events":[{"key":"AVs61KOul-sCHwhjXy0Q","category":"QUALITY_PROFILE","name":"Changes in 'Sonar way' (XML)"}]},{"key":"AVs1ruWON8SVfmPh8YRp","date":"2017-04-04T00:15:25+0300","events":[]},{"key":"AVsy24WIN8SVfmPh8U63","date":"2017-04-03T11:03:41.0.00","events":[]},{"key":"AVsrb7cWFy9QwCKalHYi","date":"2017-04-02T00:31:59+0300","events":[]},{"key":"AVsmN-7cFy9QwCKalHSa","date":"2017-04-01T00:13:29+0300","events":[]},{"key":"AVshEc9NFy9QwCKak-sI","date":"2017-03-31T00:13:35+0300","events":[]},{"key":"AVsb-K6yFy9QwCKak2DQ","date":"2017-03-30T00:28:11.0.00","events":[]},{"key":"AVsWw3A7Fy9QwCKaksvC","date":"2017-03-29T00:12:04+0300","events":[]},{"key":"AVsRpoEDn9RoWcdXmlKA","date":"2017-03-28T00:22:22+0300","events":[]},{"key":"AVsMd6WCn9RoWcdXmkBU","date":"2017-03-27T00:13:02+0300","events":[]},{"key":"AVsHhTQun9RoWcdXmj6f","date":"2017-03-26T00:09:47+0200","events":[]},{"key":"AVsCcB6xn9RoWcdXmjtX","date":"2017-03-25T00:28:05+0200","events":[]},{"key":"AVr9PDdxn9RoWcdXmgqu","date":"2017-03-24T00:13:49+0200","events":[]},{"key":"AVr4Gxktn9RoWcdXme0H","date":"2017-03-23T00:19:34+0200","events":[]},{"key":"AVry7JNJIp-DtspFygO_","date":"2017-03-22T00:10:36+0200","events":[]},{"key":"AVrt6P22Ip-DtspFye6_","date":"2017-03-21T00:48:11+0200","events":[]},{"key":"AVroo7O9Ip-DtspFyeyY","date":"2017-03-20T00:14:50+0200","events":[]},{"key":"AVrjfB56Ip-DtspFyeuJ","date":"2017-03-19T00:13:24+0200","events":[]},{"key":"AVreqkKQIp-DtspFyetG","date":"2017-03-18T01:45:17+0200","events":[]},{"key":"AVrZMhjZIp-DtspFyel0","date":"2017-03-17T00:15:39+0200","events":[]},{"key":"AVrUBhLDIp-DtspFyeeX","date":"2017-03-16T00:10:14+0200","events":[]},{"key":"AVrS0741Ip-DtspFyedI","date":"2017-03-15T18:35:36+0200","events":[{"key":"AVrS0--uIp-DtspFyedJ","category":"QUALITY_GATE","name":"Red","description":"Coverage on New Code < 50 since previous version (2017i - 2017 Feb 05)"}]},{"key":"AVrJ2A05VRHNo1h8S_jC","date":"2017-03-14T00:43:22+0200","events":[]},{"key":"AVrElFa2VRHNo1h8SvZp","date":"2017-03-13T00:11:01.0.00","events":[]},{"key":"AVq_aiPeVRHNo1h8SvTK","date":"2017-03-12T00:07:23+0200","events":[]},{"key":"AVq6SqnLJvSLzRiKI2Di","date":"2017-03-11T00:15:01.0.00","events":[]},{"key":"AVq1KYOcJvSLzRiKI14Y","date":"2017-03-10T00:19:25+0200","events":[]},{"key":"AVqv-itUJvSLzRiKIun-","date":"2017-03-09T00:10:59+0200","events":[]},{"key":"AVqbXABmJvSLzRiKHwXF","date":"2017-03-05T00:05:43+0200","events":[]},{"key":"AVp3YJo6J2GOl-mlN1z3","date":"2017-02-26T00:24:23+0200","events":[]},{"key":"AVoLQMpRf8YVno5NlDN1","date":"2017-02-05T00:30:14+0200","events":[{"key":"AVrOYehuPS27Trlx-2jR","category":"VERSION","name":"2017i"}]},{"key":"AVnnLCCGf8YVno5Nkpfj","date":"2017-01-29T00:21:20+0200","events":[]},{"key":"AVlRy_i1k-GtPBMdmP2-","date":"2016-12-31T00:13:07+0200","events":[]},{"key":"AViivg1Tk-GtPBMdkBRL","date":"2016-11-27T00:23:45+0200","events":[]},{"key":"AVgC4zPq4Rc2fYqxkEuf","date":"2016-10-27T00:25:15+0300","events":[{"key":"AVrOUS04PS27Trlx-1kk","category":"QUALITY_PROFILE","name":"Changes in 'Default  C# - Roslyn' (C#)"}]},{"key":"AVeWwDWmIkT_s3h6gifB","date":"2016-10-06T00:28:09+0300","events":[{"key":"AVrOUS0xPS27Trlx-1iD","category":"QUALITY_PROFILE","name":"Use 'Default  C# - Roslyn' (C#)"},{"key":"AVrOUS0xPS27Trlx-1iE","category":"QUALITY_PROFILE","name":"Stop using 'Default  C#' (C#)"}]},{"key":"AVeBmBDMQZITWZVHh9zK","date":"2016-09-28T00:23:51.0.00","events":[{"key":"AVrOUS7OPS27Trlx-1wx","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBmFavQZITWZVHiS_o","date":"2016-09-13T00:54:16+0300","events":[{"key":"AVrOUS6vPS27Trlx-1uv","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBl0-KQZITWZVHhEg9","date":"2016-09-10T00:21:11.0.00","events":[{"key":"AVrOUS9IPS27Trlx-2E5","category":"VERSION","name":"2017"}]},{"key":"AVeBlu8RQZITWZVHgrxP","date":"2016-08-27T10:17:49+0300","events":[{"key":"AVrOUS-fPS27Trlx-2RH","category":"QUALITY_PROFILE","name":"Changes in 'Default MSBuild Profile' (MSBuild)"}]},{"key":"AVeBmIQbQZITWZVHif80","date":"2016-07-13T00:38:16+0300","events":[{"key":"AVrOUS-CPS27Trlx-2N6","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBmH-iQZITWZVHiecy","date":"2016-06-16T00:23:24+0300","events":[{"key":"AVrOUS6YPS27Trlx-1rN","category":"QUALITY_PROFILE","name":"Changes in 'Default MSBuild Profile' (MSBuild)"}]},{"key":"AVeBmILlQZITWZVHife5","date":"2016-06-13T00:23:54+0300","events":[{"key":"AVrOUS7OPS27Trlx-1xc","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBlxeKQZITWZVHg1GL","date":"2016-05-29T00:14:20+0300","events":[]},{"key":"AVeBl2erQZITWZVHhL0o","date":"2016-04-25T00:25:31+0300","events":[]},{"key":"AVeBmDaHQZITWZVHiIja","date":"2016-03-28T00:42:25+0300","events":[]},{"key":"AVeBlt6OQZITWZVHgmmQ","date":"2016-02-15T00:22:04+0200","events":[{"key":"AVrOUS_wPS27Trlx-2du","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBl8YzQZITWZVHhnPb","date":"2016-02-14T00:36:45+0200","events":[{"key":"AVrOUTAJPS27Trlx-2hL","category":"QUALITY_PROFILE","name":"Changes in 'Default MSBuild Profile' (MSBuild)"}]},{"key":"AVeBmGPcQZITWZVHiV-d","date":"2016-02-01T00:36:36+0200","events":[{"key":"AVrOUS9kPS27Trlx-2LH","category":"VERSION","name":"2016i"}]},{"key":"AVeBmF0TQZITWZVHiUuC","date":"2016-01-14T00:18:24+0200","events":[{"key":"AVrOUS6VPS27Trlx-1qD","category":"QUALITY_PROFILE","name":"Stop using 'CopXaml' (xaml)"}]},{"key":"AVeBmBV3QZITWZVHh_IJ","date":"2015-12-10T00:16:13+0200","events":[{"key":"AVrOUTAJPS27Trlx-2hz","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBlt8iQZITWZVHgmsk","date":"2015-12-08T00:06:17+0200","events":[{"key":"AVrOUS-gPS27Trlx-2S1","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBl136QZITWZVHhJGh","date":"2015-12-05T00:26:27+0200","events":[{"key":"AVrOUS6UPS27Trlx-1o7","category":"QUALITY_PROFILE","name":"Changes in 'Default MSBuild Profile' (MSBuild)"}]},{"key":"AVeBl2rtQZITWZVHhMtS","date":"2015-12-04T00:23:43+0200","events":[{"key":"AVrOUS8iPS27Trlx-2CL","category":"QUALITY_PROFILE","name":"Changes in 'Default MSBuild Profile' (MSBuild)"},{"key":"AVrOUS8iPS27Trlx-2CM","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBly59QZITWZVHg696","date":"2015-12-02T00:39:09+0200","events":[{"key":"AVrOUS7nPS27Trlx-12H","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBmAzAQZITWZVHh8hB","date":"2015-12-01T09:52:10+0200","events":[{"key":"AVrOUS9IPS27Trlx-2G5","category":"QUALITY_PROFILE","name":"Changes in 'Default MSBuild Profile' (MSBuild)"},{"key":"AVrOUS9JPS27Trlx-2G6","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBmEiYQZITWZVHiOl5","date":"2015-11-26T00:31:31.0.00","events":[{"key":"AVrOUS-gPS27Trlx-2Tl","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBmGhYQZITWZVHiXXS","date":"2015-11-23T09:55:51.0.00","events":[{"key":"AVrOUS-7PS27Trlx-2Xl","category":"QUALITY_PROFILE","name":"Changes in 'Default  C#' (C#)"}]},{"key":"AVeBmGhYQZITWZVHiXV8","date":"2015-11-22T11:20:11+0200","events":[{"key":"AVrOUS7nPS27Trlx-12E","category":"VERSION","name":"Work"},{"key":"AVrOUS7nPS27Trlx-12F","category":"QUALITY_PROFILE","name":"Use 'Default  C#' (C#)"},{"key":"AVrOUS7nPS27Trlx-12G","category":"QUALITY_PROFILE","name":"Stop using 'Resharper And StyleCop And Cop' (C#)"}]},{"key":"AVeBl2M0QZITWZVHhKyz","date":"2015-06-01T04:14:31.0.00","events":[]},{"key":"AVeBluCdQZITWZVHgnJv","date":"2015-05-26T03:49:29+0300","events":[{"key":"AVrOUS7pPS27Trlx-13x","category":"QUALITY_PROFILE","name":"Changes in 'Resharper And StyleCop And Cop' (C#)"}]},{"key":"AVeBlxyZQZITWZVHg2ED","date":"2015-05-25T04:16:34+0300","events":[{"key":"AVrOUS-BPS27Trlx-2Mo","category":"QUALITY_PROFILE","name":"Changes in 'Resharper And StyleCop And Cop' (C#)"}]},{"key":"AVeBlsB4QZITWZVHgjEo","date":"2015-04-06T01:01:00+0300","events":[]},{"key":"AVeBmAN6QZITWZVHh55c","date":"2015-03-31T04:37:42+0300","events":[{"key":"AVrOUS7_PS27Trlx-14q","category":"QUALITY_PROFILE","name":"Changes in 'Resharper And StyleCop And Cop' (C#)"}]},{"key":"AVeBmEUeQZITWZVHiNTC","date":"2015-03-19T14:07:41+0200","events":[{"key":"AVrOUS9kPS27Trlx-2Kr","category":"QUALITY_PROFILE","name":"Changes in 'Resharper And StyleCop And Cop' (C#)"}]},{"key":"AVeBl3hPQZITWZVHhQoZ","date":"2015-03-17T05:10:36+0200","events":[{"key":"AVrOUS8OPS27Trlx-1-U","category":"QUALITY_PROFILE","name":"Changes in 'Resharper And StyleCop And Cop' (C#)"}]},{"key":"AVeBl27KQZITWZVHhOA-","date":"2015-03-12T17:24:37+0200","events":[{"key":"AVrOUS6wPS27Trlx-1wR","category":"QUALITY_PROFILE","name":"Changes in 'Resharper And StyleCop And Cop' (C#)"}]},{"key":"AVeBl2tMQZITWZVHhM2M","date":"2015-03-03T03:20:49+0200","events":[{"key":"AVrOUS8PPS27Trlx-1_9","category":"QUALITY_PROFILE","name":"Changes in 'Resharper And StyleCop And Cop' (C#)"}]},{"key":"AVeBl96FQZITWZVHhuk-","date":"2015-02-28T03:21:31.0.00","events":[{"key":"AVrOUS-7PS27Trlx-2W5","category":"QUALITY_PROFILE","name":"Stop using 'Cop C# Resharper' (C#)"},{"key":"AVrOUS-7PS27Trlx-2W6","category":"QUALITY_PROFILE","name":"Use 'Resharper And StyleCop And Cop' (C#)"}]},{"key":"AVeBmDx_QZITWZVHiKhh","date":"2015-02-26T09:59:36+0200","events":[{"key":"AVrOUS_xPS27Trlx-2ek","category":"QUALITY_PROFILE","name":"Changes in 'Cop C# Resharper' (C#)"}]},{"key":"AVeBl22OQZITWZVHhNly","date":"2015-02-03T04:15:06+0200","events":[{"key":"AVrOUS8OPS27Trlx-19L","category":"VERSION","name":"21.1"}]},{"key":"AVeBmB_0QZITWZVHiCFB","date":"2015-01-16T03:08:50+0200","events":[{"key":"AVrOUS9kPS27Trlx-2Ke","category":"QUALITY_PROFILE","name":"Changes in 'Cop C# Resharper' (C#)"}]},{"key":"AVeBl3PJQZITWZVHhPhv","date":"2014-12-01T12:25:12+0200","events":[]},{"key":"AVeBl1suQZITWZVHhIY-","date":"2014-11-04T14:18:57+0200","events":[{"key":"AVrOUS-7PS27Trlx-2WE","category":"QUALITY_PROFILE","name":"Changes in 'Cop C# Resharper' (C#)"}]},{"key":"AVeBl9pvQZITWZVHhtg5","date":"2014-10-26T00:32:06+0300","events":[{"key":"AVrOUS8OPS27Trlx-19t","category":"QUALITY_PROFILE","name":"Changes in 'CopXaml' (xaml)"}]},{"key":"AVeBmCflQZITWZVHiEH5","date":"2014-10-24T11:02:57+0300","events":[{"key":"AVrOUS-CPS27Trlx-2N3","category":"QUALITY_PROFILE","name":"Changes in 'Cop C# Resharper' (C#)"}]},{"key":"AVeBl6MbQZITWZVHhdTC","date":"2014-10-08T01:00:21.0.00","events":[{"key":"AVrOUS8PPS27Trlx-1-m","category":"QUALITY_PROFILE","name":"Changes in 'CopXaml' (xaml)"},{"key":"AVrOUS8PPS27Trlx-1_r","category":"QUALITY_PROFILE","name":"Changes in 'Sonar way' (XML)"},{"key":"AVrOUS8PPS27Trlx-1_s","category":"QUALITY_PROFILE","name":"Use 'Default MSBuild Profile' (msbuild)"}]},{"key":"AVeBl2WJQZITWZVHhLdB","date":"2014-10-04T10:38:35+0300","events":[{"key":"AVrOUS_VPS27Trlx-2Yl","category":"QUALITY_PROFILE","name":"Changes in 'Cop C# Resharper' (C#)"}]},{"key":"AVeBl2WJQZITWZVHhLZe","date":"2014-10-04T10:27:26+0300","events":[{"key":"AVrOUTAJPS27Trlx-2gz","category":"QUALITY_PROFILE","name":"Stop using 'Cop C# rules' (C#)"},{"key":"AVrOUTAJPS27Trlx-2g0","category":"QUALITY_PROFILE","name":"Use 'Cop C# Resharper' (C#)"},{"key":"AVrOUTAJPS27Trlx-2g1","category":"QUALITY_PROFILE","name":"Use 'Sonar way' (XML)"}]},{"key":"AVeBmDA8QZITWZVHiGx0","date":"2014-09-07T00:26:04+0300","events":[{"key":"AVrOUS9IPS27Trlx-2Fa","category":"QUALITY_PROFILE","name":"Changes in 'CopXaml' (xaml)"}]},{"key":"AVeBl_fzQZITWZVHh2PC","date":"2014-08-04T00:18:41+0300","events":[]},{"key":"AVeBmBBtQZITWZVHh9pQ","date":"2014-07-07T14:29:41+0300","events":[]},{"key":"AVeBmBgXQZITWZVHh_8q","date":"2014-06-15T14:45:13+0300","events":[{"key":"AVrOUS6ZPS27Trlx-1sS","category":"QUALITY_PROFILE","name":"Cop C# rules version 12","description":"Cop C# rules version 12 is used instead of Cop C# rules version 10"}]},{"key":"AVeBl_zKQZITWZVHh334","date":"2014-05-22T12:29:23+0300","events":[]},{"key":"AVeBl3jCQZITWZVHhQyP","date":"2014-04-07T14:34:56+0300","events":[]},{"key":"AVeBl88gQZITWZVHhp89","date":"2014-03-03T14:42:39+0200","events":[]},{"key":"AVeBl_sRQZITWZVHh3VF","date":"2014-02-07T01:15:34+0200","events":[{"key":"AVrOUS-fPS27Trlx-2RP","category":"VERSION","name":"20.1"}]},{"key":"AVeBl78LQZITWZVHhk-e","date":"2014-01-24T11:59:17+0200","events":[{"key":"AVrOUS9IPS27Trlx-2F2","category":"QUALITY_PROFILE","name":"Cop C# rules version 10","description":"Cop C# rules version 10 is used instead of Default  C# version 4"}]}]} """>
type CreateVersionOk = JsonProvider<""" {"event":{"key":"AVtEWEj50o2TcnNKPpPO","analysis":"AVp3YJo6J2GOl-mlN1z3","category":"VERSION","name":"test"}} """>
type CreateVersionError = JsonProvider<""" {"errors":[{"msg":"The 'analysis' parameter is missing"}]} """>

let GetAnalysisId(conf : ISonarConfiguration, projectIn : Resource, date:DateTime, httpconnector : IHttpSonarConnector) =
    if conf.SonarVersion < 6.3 then
        null, DateTime.Now, "Not available in this version of sonar"
    else
        let rec FindDate(page:int) = 
            let url = sprintf "/api/project_analyses/search?project=%s&p=%i" projectIn.Key page
            let responsecontent = httpconnector.HttpSonarGetRequest(conf, url)

            let data = ProjectAnalysis.Parse(responsecontent)
            let isFound = data.Analyses |> Seq.tryFind (fun c -> date >= c.Date.DateTime.Value.DateTime)
            match isFound with
            | Some date -> date.Key, date.Date.DateTime.Value.DateTime, ""
            | _ -> 
                if data.Paging.Total = data.Paging.PageSize then
                    FindDate(page + 1)
                else
                    null, DateTime.Now, "Analysis not found"

        FindDate(1)

let CreateVersion(conf : ISonarConfiguration, id:string, name : string, httpconnector : IHttpSonarConnector) =
    
    if conf.SonarVersion < 6.3 then
        "Not available in this version of sonar"
    else

        let url = sprintf "/api/project_analyses/create_event"
        let options = Map.empty.Add("analysis", id).Add("category", "VERSION").Add("name", name)
        let responsecontent = httpconnector.HttpSonarPostRequest(conf, url, options)

        if responsecontent.StatusCode = Net.HttpStatusCode.OK then
            "ok"
        else
            CreateVersionError.Parse(responsecontent.Content).Errors.[0].Msg