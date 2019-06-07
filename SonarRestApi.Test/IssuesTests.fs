namespace SonarRestService.Test

open NUnit.Framework
open Foq
open System.IO
open SonarRestService
open SonarRestService.Types
open SonarRestServiceImpl
open System.Reflection
open System.Threading

type IssuesTests() =
    let assemblyRunningPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString()
    let mockLogger =
        Mock<IRestLogger>()
            .Create()
    let cancelMonitors = new CancellationTokenSource()
    
    [<Test>]
    member test.``Should Get Corret Number of Issues In Component`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), any()) @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/issuessearchbycomponent.txt"))
                .Create()

        let service = SonarService(mockHttpReq)
        let issues = (service :> ISonarRestService).GetIssuesInResource(conf, "groupid:projectid:directory/file.cpp", cancelMonitors.Token, mockLogger).GetAwaiter().GetResult()
        Assert.That(issues.Count, Is.EqualTo(59))
  
    [<Test>]
    member test.``Should Get Issues in Project by user`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), any()) @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/issuesByUser.txt"))
                .Create()

        let service = SonarService(mockHttpReq)
        let issues = (service :> ISonarRestService).GetIssuesByAssigneeInProject(conf, "groupid:projectid:directory/file.cpp", "jocs1", cancelMonitors.Token, mockLogger).GetAwaiter().GetResult()
        Assert.That(issues.Count, Is.EqualTo(2))

    [<Test>]
    member test.``Should Get Issues In Project by user using old format`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "login1", "password", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(conf, "/api/reviews?projects=groupid:projectid&assignees=login1") @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/reviewByUser.txt"))
                .Setup(fun x -> <@ x.HttpSonarGetRequest(conf, "/api/issues/search?componentRoots=groupid:projectid&assignees=login1") @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/NonExistentPage.xml"))
                .Create()

        let service = SonarService(mockHttpReq)
        let issues = (service :> ISonarRestService).GetIssuesByAssigneeInProject(conf, "groupid:projectid", "login1", cancelMonitors.Token, mockLogger).GetAwaiter().GetResult()
        Assert.That(issues.Count, Is.EqualTo(5))

    [<Test>]
    member test.``Should Get All Issues In Project`` () =
        let conf = ConnectionConfiguration("http://localhost:9000", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), any()) @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/issuessearchbycomponent.txt"))
                .Create()

        let service = SonarService(mockHttpReq)
        let issues = (service :> ISonarRestService).GetIssuesForProjects(conf, "project:id", cancelMonitors.Token, mockLogger).GetAwaiter().GetResult()
        Assert.That(issues.Count, Is.EqualTo(59))
