﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpSonarConnector.fs" company="Copyright © 2014 Tekla Corporation. Tekla is a Trimble Company">
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

open System
open System.Net
open System.Web
open System.Text
open System.IO
open SonarRestService.Types
open RestSharp
open RestSharp.Authenticators

type JsonSonarConnector() = 

    let userRoamingFile =
        // required to call https://sonarqube.com 
        // not convinced on the scalability of this approach should Tls12 become vulnerable
        ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VSSonarExtension\\restCalls.log");

    interface IHttpSonarConnector with
        member this.HttpSonarPutRequest(userConf : ISonarConfiguration, url : string, data : Map<string, string>) =

            let client = new RestClient(userConf.Hostname)
            client.Authenticator <- new HttpBasicAuthenticator(userConf.Username, userConf.Password)
            let request = new RestRequest(url, Method.PUT);

            for elem in data do
                request.AddParameter(elem.Key, elem.Value) |> ignore

            //request.AddHeader(HttpRequestHeader.Accept.ToString(), "text/xml") |> ignore
            request.RequestFormat <- DataFormat.Json

            client.Execute(request)

        member this.HttpSonarPostRequest(userConf : ISonarConfiguration, url : string, data : Map<string, string>) =

            let client = new RestClient(userConf.Hostname)
            client.Authenticator <- new HttpBasicAuthenticator(userConf.Username, userConf.Password)
            let request = new RestRequest(url, Method.POST);

            for elem in data do
                request.AddParameter(elem.Key, elem.Value) |> ignore

            //request.AddHeader(HttpRequestHeader.Accept.ToString(), "text/xml") |> ignore
            request.RequestFormat <- DataFormat.Json

            client.Execute(request)

        member this.HttpSonarPostRequestDic(userConf : ISonarConfiguration, url : string, data : System.Collections.Generic.Dictionary<string, string>) =

            let client = new RestClient(userConf.Hostname)
            client.Authenticator <- new HttpBasicAuthenticator(userConf.Username, userConf.Password)
            let request = new RestRequest(url, Method.POST);

            for elem in data do
                request.AddParameter(elem.Key, elem.Value) |> ignore

            //request.AddHeader(HttpRequestHeader.Accept.ToString(), "text/xml") |> ignore
            request.RequestFormat <- DataFormat.Json

            client.Execute(request)

        member this.HttpSonarGetRequest(userConf : ISonarConfiguration, url : string) =
            if obj.ReferenceEquals(userConf, null) then
                ""
            else
                let req = HttpWebRequest.Create(userConf.Hostname + url) :?> HttpWebRequest 
                req.Method <- "GET"
                req.ContentType <- "text/json"

                if userConf.Username <> ""  then
                    let auth = "Basic " + (userConf.Username + ":" + userConf.Password |> Encoding.UTF8.GetBytes |> Convert.ToBase64String)
                    req.Headers.Add("Authorization", auth)
                
                let addLine (line:string) =
                    if not(String.IsNullOrEmpty(Environment.GetEnvironmentVariable("VSSONAREXTENSIONDEBUG"))) then
                        use wr = new StreamWriter(userRoamingFile, true)
                        wr.WriteLine(line)
                                
                // read data
                try
                    let rsp = req.GetResponse()
                    use stream = rsp.GetResponseStream()
                    use reader = new StreamReader(stream)
                    let timeNow = System.DateTime.Now.ToString()

                    addLine (sprintf """ [%s] : %s """ timeNow url)                
                    let data = reader.ReadToEnd()
                    if not(String.IsNullOrEmpty(Environment.GetEnvironmentVariable("VSSONAREXTENSIONDEBUGVERBOSE"))) then
                        addLine (sprintf """ [%s] : %s """ timeNow data)
                    data
                with
                 | ex -> 
                    let timeNow = System.DateTime.Now.ToString()
                    addLine (sprintf """ [%s] : %s """ timeNow url)
                    addLine (sprintf """ [%s] : Error: %s""" timeNow ex.Message)
                    addLine (sprintf """        StackTrace: %s""" ex.StackTrace)
                    raise ex

        member this.HttpSonarRequest(userconf : ISonarConfiguration, urltosue : string, methodin : Method) =
            let client = new RestClient(userconf.Hostname)
            client.Authenticator <- new HttpBasicAuthenticator(userconf.Username, userconf.Password)
            let request = new RestRequest(urltosue, methodin)
            //request.AddHeader(HttpRequestHeader.Accept.ToString(), "text/xml") |> ignore
            request.RequestFormat <- DataFormat.Json
            client.Execute(request)