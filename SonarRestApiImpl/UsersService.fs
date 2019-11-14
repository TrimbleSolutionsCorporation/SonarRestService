module UsersService

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

type JsonUsers = JsonProvider<""" {"users":[{"login":"user1","name":"","active":true},{"login":"admin","name":"Administrator","active":true},{"login":"user2","name":"Real Name","active":true,"email":"real.name@org.com"}]} """>

type TeamsJson = JsonProvider<"""{"teams":[{"team_name":"Bat Team","users":[{"name":"Jorge Costa","alias":["jorge.costa@trimble.com","jocs@trimble.com","jocs@trimble.com"]},{"name":"Ganga","alias":["jorge.costa@trimble.com","jocs@trimble.com","jocs@trimble.com"]}]},{"team_name":"Bat Team","users":[{"name":"Jorge Costa","alias":["jorge.costa@trimble.com","jocs@trimble.com","jocs@trimble.com"]},{"name":"Ganga","alias":["jorge.costa@trimble.com","jocs@trimble.com","jocs@trimble.com"]}]}]}""">

let getUserListFromXmlResponse(responsecontent : string) =
    let data = JsonUsers.Parse(responsecontent) 

    let userList = new System.Collections.Generic.List<User>()
    for user in data.Users do
        let newUser = new User()

        if not(obj.ReferenceEquals(user.Email, null)) then
            newUser.Email <- user.Email.Value
        else
            newUser.Email <- ""

        match user.Name with
        | None -> newUser.Name <- ""
        | Some c -> newUser.Name <- c

        let login = user.Login
        System.Diagnostics.Debug.Write(login)
        newUser.Active <- user.Active
        newUser.Login <- user.Login

        userList.Add(newUser)

    userList

let matchUserNames(nameonein:string, nametwoin:string) = 
    let nameone = nameonein.ToLower()
    let nametwo = nametwoin.ToLower()
    if nameone = nametwo then
        true
    else
        let elementsNameOne = nameone.Split()
        let elementsNameTwo = nametwo.Split()
        if elementsNameTwo.Length <> elementsNameOne.Length then
            false
        else
            let mutable increment = 0
            elementsNameOne |> Seq.iter (fun nameone -> (if elementsNameTwo.Contains(nameone) then increment <- increment + 1))
            if increment = elementsNameOne.Count() then
                true
            else
                false


let GetTeams(users : System.Collections.Generic.IEnumerable<User>, teamsFile:string) =
    let teams = new System.Collections.Generic.List<Team>()

    if File.Exists(teamsFile) then
        let HandleTeam(team:TeamsJson.Team) = 
            let teamToAdd = Team()
            teamToAdd.Users <- System.Collections.Generic.List<User>()
            teamToAdd.Name <- team.TeamName
            
            let AddToTeamIfNotThere(user:TeamsJson.User) =
                let userdata = user.Name
                let isFoundInUsers = users |> Seq.tryFind(fun userInSonar -> matchUserNames(userInSonar.Name, user.Name))

                if isFoundInUsers.IsSome && (teamToAdd.Users |> Seq.tryFind (fun element -> element.Name.Equals(isFoundInUsers.Value.Name))).IsNone then
                    isFoundInUsers.Value.Team <- team.TeamName
                    user.Alias |> Seq.iter (fun elem -> isFoundInUsers.Value.AddionalEmails.Add(elem))
                    teamToAdd.Users.Add(isFoundInUsers.Value)

            team.Users
            |> Seq.iter (fun user -> AddToTeamIfNotThere(user))

            if teamToAdd.Users.Count > 0 then
                teams.Add(teamToAdd)

        let teams = TeamsJson.Parse(File.ReadAllText(teamsFile))
        teams.Teams
        |> Seq.iter (fun team -> HandleTeam(team))
    teams

let GetUserList(newConf : ISonarConfiguration, httpconnector : IHttpSonarConnector) =
    let url = "/api/users/search?ps=500"           
    try
        let responsecontent = httpconnector.HttpSonarGetRequest(newConf, url)
        getUserListFromXmlResponse(responsecontent)
    with
     | ex -> new System.Collections.Generic.List<User>()

let AuthenticateUser(newConf : ISonarConfiguration, httpconnector : IHttpSonarConnector) =
    let url = "/api/authentication/validate"

    try
        let responsecontent = httpconnector.HttpSonarGetRequest(newConf, url)
        JsonValidateUser.Parse(responsecontent).Valid
    with
        | ex -> false
