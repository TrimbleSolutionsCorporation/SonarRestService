﻿namespace SonarRestService.Test

open NUnit.Framework
open Foq
open System.IO
open SonarRestService
open SonarRestService.Types
open SonarRestServiceImpl
open System.Reflection
open System.Threading
open System
open System.Threading.Tasks

type UserServiceTests() =
    let assemblyRunningPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString()
    let mockLogger =
        Mock<IRestLogger>()
            .Create()
    let cancelMonitors = new CancellationTokenSource()
    
    [<Test>]
    member test.``Gets empty teams if no users are found`` () =
        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Create()
        let users = System.Collections.Generic.List<User>()
        let service = SonarService(mockHttpReq)
        let teams = (service :> ISonarRestService).GetTeams(users, Path.Combine(assemblyRunningPath, "testdata", "teams.json")).GetAwaiter().GetResult()
        Assert.That(teams.Count, Is.EqualTo(0))

    [<Test>]
    member test.``Gets one teams if no users are found`` () =
        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Create()
        let users = System.Collections.Generic.List<User>()
        users.Add(new User(Name = "Jorge Costa", Login = "jocs", Email = "Jorge.Costa@email.com"))
        let service = SonarService(mockHttpReq)
        let teams = (service :> ISonarRestService).GetTeams(users, Path.Combine(assemblyRunningPath, "testdata", "teams.json")).GetAwaiter().GetResult()
        Assert.That(teams.Count, Is.EqualTo(1))

    [<Test>]
    member test.``Gets all teams if no users are found`` () =
        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Create()
        let users = System.Collections.Generic.List<User>()
        let juser = new User(Name = "Jorge Costa", Login = "jocs", Email = "Jorge.Costa@email.com")
        users.Add(juser)
        let suser = new User(Name = "Lim", Login = "lim", Email = "Lim@email.com")
        users.Add(suser)
        let service = SonarService(mockHttpReq)
        let teams = (service :> ISonarRestService).GetTeams(users, Path.Combine(assemblyRunningPath, "testdata", "teams.json")).GetAwaiter().GetResult()
        Assert.That(teams.Count, Is.EqualTo(2))
        Assert.That(juser.Team, Is.EqualTo("Bat Team"))
        Assert.That(juser.AddionalEmails.Count, Is.EqualTo(3))
        Assert.That(suser.Team, Is.EqualTo("Sold Team"))
