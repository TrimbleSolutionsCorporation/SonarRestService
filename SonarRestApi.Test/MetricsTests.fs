namespace SonarRestService.Test

open NUnit.Framework
open SonarRestService
open SonarRestService.Types
open SonarRestServiceImpl
open Foq
open System.IO

open System.Reflection

type MetricsTests() =
    let assemblyRunningPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString()
    [<Test>]
    member test.``Should Get Corret Duplications From Data`` () =
        let conf = ConnectionConfiguration("http://sonar", "jocs1", "jocs1", 4.5)

        let mockHttpReq =
            Mock<IHttpSonarConnector>()
                .Setup(fun x -> <@ x.HttpSonarGetRequest(any(), any()) @>).Returns(File.ReadAllText(assemblyRunningPath + "/testdata/duplicationsdata.txt"))
                .Create()
        let service = SonarService(mockHttpReq)
        let dups = (service :> ISonarRestService).GetDuplicationsDataInResource(conf, "groupid:projectid:directory/file.cpp")
        Assert.That(dups.Count, Is.EqualTo(66))
