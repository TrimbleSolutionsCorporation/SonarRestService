namespace SonarRestService.Test

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
    member test.``Matches names in different order`` () =
        Assert.That(UsersService.matchUserNames("Jorge Costa", "Jorge Costa"), Is.True)
        Assert.That(UsersService.matchUserNames("Jorge Costa", "Costa Jorge"), Is.True)
        Assert.That(UsersService.matchUserNames("Jorge Manuel Costa", "Manuel Costa Jorge"), Is.True)
        Assert.That(UsersService.matchUserNames("Antonio", "Jorge Costa"), Is.False)
    
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


    //[<Test>]
    member test.``UpdateUser`` () =
        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Create()

        let conf = ConnectionConfiguration("https://sonar.tekla.com", "", "", 1.0)
        let service = SonarService(JsonSonarConnector())
        let users = (service :> ISonarRestService).GetUserList(conf).GetAwaiter().GetResult()

        (service :> ISonarRestService).UpdateUserLogin(conf, "login", "login").Result |> ignore

        let parmas2 = new System.Collections.Generic.Dictionary<string, string>()
        parmas2.Add("login", "login")
        parmas2.Add("newExternalIdentity", "@trimble.com")
        parmas2.Add("newExternalProvider", "saml")
        (service :> ISonarRestService).UpdateIdentityProvider(conf, parmas2).Result |> ignore

    //[<Test>]
    member test.``Migrate Users to saml`` () =
        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Create()

        let conf = ConnectionConfiguration("https://sonar.tekla.com", "", "", 1.0)
        let service = SonarService(JsonSonarConnector())
        let users = (service :> ISonarRestService).GetUserList(conf).GetAwaiter().GetResult()

        let MigrateUser(user:User) =
            let parmas = new System.Collections.Generic.Dictionary<string, string>()

            parmas.Add("login", user.Login)
            parmas.Add("newExternalIdentity", user.Email)
            parmas.Add("newExternalProvider", "saml")
            (service :> ISonarRestService).UpdateIdentityProvider(conf, parmas).Result |> ignore
            ()

        users |> Seq.iter (fun user -> MigrateUser(user))