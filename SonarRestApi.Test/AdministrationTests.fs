﻿namespace SonarRestService.Test

open NUnit.Framework
open SonarRestService
open SonarRestService.Types
open SonarRestServiceImpl
open Foq
open System.IO
open System.Net
open System.Web

open System.Reflection

type AdministrationTests() =
   
    let assemblyRunningPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString()

    [<Test>]
    member test.``Should Get Users`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/users/search?ps=1000") @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/userList.txt"))
                .Create()

        let service = SonarService(mockHttpReq)
        let userList = (service :> ISonarRestService).GetUserList(conf)
        Assert.That(userList.Count, Is.EqualTo(3))

    [<Test>]
    member test.``Should Not Get Users for Server Less than 3.6`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/users/search") @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/NonExistentPage.xml"))
                .Create()

        let service = SonarService(mockHttpReq)
        let userList = (service :> ISonarRestService).GetUserList(conf)
        Assert.That(userList.Count, Is.EqualTo(0))

    [<Test>]
    member test.``Should Authenticate User`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/authentication/validate") @>).Returns(""" {"valid":true} """)
                .Create()

        let service = SonarService(mockHttpReq)
        Assert.That((service :> ISonarRestService).AuthenticateUser(conf), Is.True)


    [<Test>]
    member test.``Should Not Authenticate User`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/authentication/validate") @>).Returns(""" {"valid":false} """)
                .Create()

        let service = SonarService(mockHttpReq)
        Assert.That((service :> ISonarRestService).AuthenticateUser(conf), Is.False)

    [<Test>]
    member test.``Should Fail authentication When Sonar less than 3.3 so skip authetication`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/authentication/validate") @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/NonExistentPage.xml"))
                .Create()

        let service = SonarService(mockHttpReq)
        Assert.That((service :> ISonarRestService).AuthenticateUser(conf), Is.False)

    [<Test>]
    member test.``Should Get Correct server version with 3.6`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/server/version") @>).Returns("3.6")
                .Create()

        let service = SonarService(mockHttpReq)
        Assert.That((service :> ISonarRestService).GetServerInfo(conf), Is.EqualTo(3.6f))

    [<Test>]
    member test.``Should Get Correct server version with 3,6`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/server/version") @>).Returns("3.6")
                .Create()

        let service = SonarService(mockHttpReq)
        Assert.That((service :> ISonarRestService).GetServerInfo(conf), Is.EqualTo(3.6f))

    [<Test>]
    member test.``Should Get Correct server version with 3.6-SNAPSHOT`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/server/version") @>).Returns("3.6-SNAPSHOT")
                .Create()

        let service = SonarService(mockHttpReq)
        Assert.That((service :> ISonarRestService).GetServerInfo(conf), Is.EqualTo(3.6f))

    [<Test>]
    member test.``Should Get Correct server version with 3,6-SNAPSHOT`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/server/version") @>).Returns("3.6-SNAPSHOT")
                .Create()

        let service = SonarService(mockHttpReq)
        Assert.That((service :> ISonarRestService).GetServerInfo(conf), Is.EqualTo(3.6f))

    [<Test>]
    member test.``Should Get Correct server version with 3.6.1`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/server/version") @>).Returns("3.6.1")
                .Create()

        let service = SonarService(mockHttpReq)
        Assert.That((service :> ISonarRestService).GetServerInfo(conf), Is.EqualTo(3.6f))

    [<Test>]
    member test.``Should Get Correct server version with 3.6.1-SNAPSHOT`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), "/api/server/version") @>).Returns("3.6.1-SNAPSHOT")
                .Create()

        let service = SonarService(mockHttpReq)
        Assert.That((service :> ISonarRestService).GetServerInfo(conf), Is.EqualTo(3.6f))

    [<Test>]
    member test.``Get Plugins`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "admin", "admin", 5.3)
        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), any()) @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/plugindata.txt"))
                .Create()

        let service = SonarService(mockHttpReq)
        Assert.That((service :> ISonarRestService).GetInstalledPlugins(conf).Count, Is.EqualTo(17))


    [<Test>]
    member test.``Apply Permissions Template Fails When Project Not Found`` () =
        let conf = ConnectionConfiguration("http://sonar", "admin", "admin", 5.3)
        let response = new RestSharp.RestResponse()
        response.StatusCode <- HttpStatusCode.NoContent

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarPostRequest(any(), any(), any()) @>).Returns(response)
                .Create()

        let service = SonarService(mockHttpReq)
        let errormessage = (service :> ISonarRestService).ApplyPermissionTemplateToProject(conf, "projectKey", "projectname")
        Assert.That(errormessage, Is.EqualTo("Unable to apply or find template : projectname Value cannot be null.\r\nParameter name: s"))

    [<Test>]
    member test.``Apply Permissions Template Ok`` () =
        let conf = ConnectionConfiguration("http://sonar", "admin", "admin", 5.3)
        let response = new RestSharp.RestResponse()
        response.StatusCode <- HttpStatusCode.NoContent

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarPostRequest(any(), any(), any()) @>).Returns(response)
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), any()) @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/permissiontemplate.txt"))
                .Create()

        let service = SonarService(mockHttpReq)
        let errormessage = (service :> ISonarRestService).ApplyPermissionTemplateToProject(conf, "projectKey", "thisisname")
        Assert.That(errormessage, Is.EqualTo(""))