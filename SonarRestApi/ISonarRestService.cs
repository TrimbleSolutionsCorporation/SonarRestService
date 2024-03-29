﻿namespace SonarRestService
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Types;

    /// <summary>
    /// sonar rest service
    /// </summary>
    public interface ISonarRestService
    {
        /// <summary>
        /// Gets the issues by assignee in project.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="projectKey">The project key.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>
        /// returns list of issues
        /// </returns>
        Task<List<Issue>> GetIssuesByAssigneeInProject(ISonarConfiguration config, string projectKey, string userId, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Gets all issues by assignee.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>Task list of issues</returns>
        Task<List<Issue>> GetAllIssuesByAssignee(ISonarConfiguration config, string userId, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Gets the issues for projects.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="projectKey">The project key.</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">logger</param>
        /// <returns></returns>
        Task<List<Issue>> GetIssuesForProjects(ISonarConfiguration config, string projectKey, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Gets the issues for projects created after date.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="projectKey">The project key.</param>
        /// <param name="time">The time.</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">logger</param>
        /// <returns></returns>
        Task<List<Issue>> GetIssuesForProjectsCreatedAfterDate(ISonarConfiguration config, string projectKey, DateTime time, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Gets the issues in resource.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">logger</param>
        /// <returns></returns>
        Task<List<Issue>> GetIssuesInResource(ISonarConfiguration config, string resourceKey, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="query">The query.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">logger</param>
        /// <returns></returns>
        Task<List<Issue>> GetIssues(ISonarConfiguration config, string query, string projectId, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Gets the projects.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        List<SonarProject> GetProjects(ISonarConfiguration config);

        /// <summary>
        /// Provisions the project.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="branch">The branch.</param>
        /// <returns></returns>
        string ProvisionProject(ISonarConfiguration config, string key, string name, string branch);

        /// <summary>
        /// Comments the on issues.
        /// </summary>
        /// <param name="newConf">The new conf.</param>
        /// <param name="issues">The issues.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="logger">logger</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> CommentOnIssues(ISonarConfiguration newConf, IEnumerable<Issue> issues, string comment, IRestLogger logger, CancellationToken token);

        /// <summary>
        /// Res the open issues.
        /// </summary>
        /// <param name="Conf">The conf.</param>
        /// <param name="issues">The issues.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="logger">logger</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> ReOpenIssues(ISonarConfiguration Conf, List<Issue> issues, string comment, IRestLogger logger, CancellationToken token);

        /// <summary>
        /// Confirms the issues.
        /// </summary>
        /// <param name="newConf">The new conf.</param>
        /// <param name="issues">The issues.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="logger">logger</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> ConfirmIssues(ISonarConfiguration newConf, IEnumerable<Issue> issues, string comment, IRestLogger logger, CancellationToken token);

        /// <summary>
        /// do not confirm issues.
        /// </summary>
        /// <param name="newConf">The new conf.</param>
        /// <param name="issues">The issues.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="logger">logger</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> UnConfirmIssues(ISonarConfiguration newConf, IEnumerable<Issue> issues, string comment, IRestLogger logger, CancellationToken token);

        /// <summary>
        /// Marks the issues as false positive.
        /// </summary>
        /// <param name="newConf">The new conf.</param>
        /// <param name="issues">The issues.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="logger">logger</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> MarkIssuesAsFalsePositive(ISonarConfiguration newConf, IEnumerable<Issue> issues, string comment, IRestLogger logger, CancellationToken token);

        /// <summary>
        /// Marks the issues as false positive.
        /// </summary>
        /// <param name="newConf">The new conf.</param>
        /// <param name="issues">The issues.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="logger">logger</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> MarkIssuesAsWontFix(ISonarConfiguration newConf, IEnumerable<Issue> issues, string comment, IRestLogger logger, CancellationToken token);

        /// <summary>
        /// resolve issues
        /// </summary>
        /// <param name="newConf"></param>
        /// <param name="issues"></param>
        /// <param name="comment"></param>
        /// <param name="logger">logger</param>
        /// <param name="token">The token.</param>
        /// <returns>true if ok</returns>
        Task<bool> ResolveIssues(ISonarConfiguration newConf, IEnumerable<Issue> issues, string comment, IRestLogger logger, CancellationToken token);

        /// <summary>
        /// Assigns the issues to user.
        /// </summary>
        /// <param name="newConf">The new conf.</param>
        /// <param name="issues">The issues.</param>
        /// <param name="user">The user.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="logger">logger</param>
        /// <param name="token">The token.</param>
        /// <returns>true if ok</returns>
        Task<bool> AssignIssuesToUser(ISonarConfiguration newConf, IEnumerable<Issue> issues, User user, string comment, IRestLogger logger, CancellationToken token);

        /// <summary>
        /// Plans the issues.
        /// </summary>
        /// <param name="newConf">The new conf.</param>
        /// <param name="issues">The issues.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <param name="logger">logger</param>
        /// <param name="token">The token.</param>
        /// <returns>status code</returns>
        Task<bool> PlanIssues(ISonarConfiguration newConf, IEnumerable<Issue> issues, string planId, IRestLogger logger, CancellationToken token);

        /// <summary>
        /// unplan issues.
        /// </summary>
        /// <param name="newConf">The new conf.</param>
        /// <param name="issues">The issues.</param>
        /// <param name="logger">logger</param>
        /// <param name="token">The token.</param>
        /// <returns>status of operation</returns>
        Task<bool> UnPlanIssues(ISonarConfiguration newConf, IEnumerable<Issue> issues, IRestLogger logger, CancellationToken token);

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <returns>users</returns>
        Task<List<User>> GetUserList(ISonarConfiguration conf);

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="data">user data</param>
        /// <returns>users</returns>
        Task<HttpStatusCode> UpdateIdentityProvider(ISonarConfiguration conf, System.Collections.Generic.Dictionary<string, string> data);

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="data">user data</param>
        /// <returns>users</returns>
        Task<HttpStatusCode> UpdateUserData(ISonarConfiguration conf, System.Collections.Generic.Dictionary<string, string> data);

        /// <summary>
        /// update login
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="oldLogin">oldlogin</param>
        /// <param name="newLogin">newlogin</param>
        /// <returns>stats</returns>
        Task<HttpStatusCode> UpdateUserLogin(ISonarConfiguration conf, string oldLogin, string newLogin);
        
        /// <summary>
        /// Create a teams from a team list file
        /// </summary>
        /// <param name="availableUsers"></param>
        /// <param name="userListFile">file with users</param>
        /// <returns>teams</returns>
        Task<List<Team>> GetTeams(IEnumerable<User> availableUsers, string userListFile);

        /// <summary>
        /// Gets teams files
        /// </summary>
        /// <param name="userListFile">user list file</param>
        /// <returns>list of teams</returns>
        Task<List<Team>> GetTeamsFile(string userListFile);

        /// <summary>
        /// Gets the available tags.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>
        /// tags
        /// </returns>
        Task<List<string>> GetAvailableTags(ISonarConfiguration conf, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Sets the issue tags.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="issue">The issue.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>
        /// status of operation
        /// </returns>
        Task<string> SetIssueTags(
            ISonarConfiguration conf,
            Issue issue,
            List<string> tags,
            CancellationToken token,
            IRestLogger logger);

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <returns>ok for auth</returns>
        Task<bool> AuthenticateUser(ISonarConfiguration conf);

        /// <summary>
        /// Gets the resources data.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        List<Resource> GetResourcesData(ISonarConfiguration conf, string key);

        /// <summary>
        /// Searches the component.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="filterBranches">if set to <c>true</c> [filter branches].</param>
        /// <param name="masterBranch">current master branch</param>
        /// <returns></returns>
        List<Resource> SearchComponent(ISonarConfiguration conf, string searchString, bool filterBranches, string masterBranch);

        /// <summary>
        /// Gets the projects list.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <returns></returns>
        List<Resource> GetProjectsList(ISonarConfiguration conf);

        /// <summary>
        /// Gets the server information.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <returns></returns>
        float GetServerInfo(ISonarConfiguration conf);

        /// <summary>
        /// Gets the coverage in resource.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        SourceCoverage GetCoverageInResource(ISonarConfiguration conf, string key);

        /// <summary>
        /// Gets the source for file resource.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Source GetSourceForFileResource(ISonarConfiguration conf, string key);

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="props">The props.</param>
        /// <param name="project">The project, in can its null returns server properties.</param>
        /// <returns>
        /// Settings for project or server
        /// </returns>
        IEnumerable<Setting> GetSettings(ISonarConfiguration props, Resource project = null);

        /// <summary>
        /// Sets the setting.
        /// </summary>
        /// <param name="props">The props.</param>
        /// <param name="setting">The setting.</param>
        /// <param name="project">The project.</param>
        /// <returns>status of the operation</returns>
        string SetSetting(ISonarConfiguration props, Setting setting, Resource project = null);

        /// <summary>
        /// Creates the version.
        /// </summary>
        /// <param name="props">The props.</param>
        /// <param name="project">The project.</param>
        /// <param name="version">The version.</param>
        /// <param name="date">creates a version after this date</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">logger</param>
        /// <returns>empty if ok</returns>
        Task<string> CreateVersion(ISonarConfiguration props, Resource project, string version, DateTime date, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Gets the coverage report on new code on leak.
        /// </summary>
        /// <param name="props">The props.</param>
        /// <param name="project">The project.</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">logger</param>
        /// <returns>coverage differential</returns>
        Task<Dictionary<string, CoverageDifferencial>> GetCoverageReportOnNewCodeOnLeak(ISonarConfiguration props, Resource project, CancellationToken token, IRestLogger logger);


        /// <summary>
        /// Gets the coverage report.
        /// </summary>
        /// <param name="props">The props.</param>
        /// <param name="project">The project.</param>
        /// <param name="token">The token.</param>
        /// <param name="logger">logger</param>
        /// <returns>coverage report, obsolete</returns>
        [Obsolete("Use GetCoverageReportOnNewCodeOnLeak")]
        Task<Dictionary<string, CoverageReport>> GetCoverageReport(ISonarConfiguration props, Resource project, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Gets the summary project report., includes new_coverage,ncloc,coverage,new_lines,ncloc_language_distribution,violations,new_violations,new_technical_debt,cognitive_complexity,complexity,lines,sqale_index
        /// </summary>
        /// <param name="props">The props.</param>
        /// <param name="project">The project.</param>
        /// <param name="token">cancelation token</param>
        /// <param name="logger">logger for sonar</param>
        /// <returns>project summary report with new_coverage,ncloc,coverage,new_lines,ncloc_language_distribution,violations,new_violations,new_technical_debt,cognitive_complexity,complexity,lines,sqale_index</returns>
        Task<Dictionary<string, ProjectSummaryReport>> GetSummaryProjectReport(ISonarConfiguration props, Resource project, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Updates the property.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="projectIn">The project in, if null it will update global property</param>
        /// <returns>empty string if OK, error message if fails</returns>
        string UpdateProperty(ISonarConfiguration conf, string id, string value, Resource projectIn);

        /// <summary>
        /// Gets the enabled rules in profile.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="language">The language.</param>
        /// <param name="profile">The profile.</param>
        /// <returns></returns>
        List<Profile> GetEnabledRulesInProfile(ISonarConfiguration conf, string language, string profile);

        /// <summary>
        /// Gets the quality profile.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="project">The resource key.</param>
        /// <returns>resource</returns>
        List<Resource> GetQualityProfile(ISonarConfiguration conf, Resource project);

        /// <summary>
        /// gets rules for profile
        /// </summary>
        /// <param name="conf">cong</param>
        /// <param name="profile">profile</param>
        /// <param name="searchDetails">get more details</param>
        void GetRulesForProfile(ISonarConfiguration conf, Profile profile, bool searchDetails);

        /// <summary>
        /// update a rule data
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="newRule">new rule</param>
        void UpdateRuleData(ISonarConfiguration conf, Rule newRule);

        /// <summary>
        /// Gets quality profiles for project
        /// </summary>
        /// <param name="conf">sonar conf</param>
        /// <param name="project">project</param>
        /// <returns>Profiles</returns>
        List<Profile> GetQualityProfilesForProject(ISonarConfiguration conf, Resource project);

        /// <summary>
        /// gets profiles for project by language
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="project">project key</param>
        /// <param name="language">language</param>
        /// <returns>Profiles</returns>
        List<Profile> GetQualityProfilesForProject(ISonarConfiguration conf, Resource project, string language);

        /// <summary>
        /// Gets all profiles
        /// </summary>
        /// <param name="conf">config</param>
        /// <returns>Profiles</returns>
        List<Profile> GetAvailableProfiles(ISonarConfiguration conf);

        /// <summary>
        /// Gets a list of rules
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="languangeKey">lang</param>
        /// <returns>rules</returns>
        List<Rule> GetRules(ISonarConfiguration conf, string languangeKey);

        /// <summary>
        /// Gets template rules
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="profile">profile</param>
        void GetTemplateRules(ISonarConfiguration conf, Profile profile);

        /// <summary>
        /// Update a rule
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="key">key</param>
        /// <param name="optionalProps">props</param>
        /// <returns>data</returns>
        List<string> UpdateRule(ISonarConfiguration conf, string key, Dictionary<string, string> optionalProps);

        /// <summary>
        /// Gets all tags
        /// </summary>
        /// <param name="conf">conf</param>
        /// <returns>tags</returns>
        List<string> GetAllTags(ISonarConfiguration conf);

        /// <summary>
        /// Update tags
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="rule">rule</param>
        /// <param name="tags">taf</param>
        /// <returns>updated tags</returns>
        List<string> UpdateTags(ISonarConfiguration conf, Rule rule, List<string> tags);

        /// <summary>
        /// Activates the rule.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="ruleKey">The rule key.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="profilekey">The profile key.</param>
        /// <returns>error message or empty if no error</returns>
        string ActivateRule(ISonarConfiguration conf, string ruleKey, string severity, string profilekey);

        /// <summary>
        /// Deletes a rule
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="rule">rule</param>
        /// <returns></returns>
        List<string> DeleteRule(ISonarConfiguration conf, Rule rule);

        /// <summary>
        /// Disable rule
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="rule">rule</param>
        /// <param name="profilekey">profile key</param>
        /// <returns>rule</returns>
        List<string> DisableRule(ISonarConfiguration conf, Rule rule, string profilekey);

        /// <summary>
        /// Creates a new rule
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="rule">rule</param>
        /// <param name="templateRule">template rule</param>
        /// <returns>rules</returns>
        List<string> CreateRule(ISonarConfiguration conf, Rule rule, Rule templateRule);

        /// <summary>
        /// Gets profiles using rules appp
        /// </summary>
        /// <param name="conf">conf</param>
        /// <returns>profiles</returns>
        List<Profile> GetProfilesUsingRulesApp(ISonarConfiguration conf);

        /// <summary>
        /// Gets profiles using rules app
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="profile">profile</param>
        /// <param name="active">active</param>
        void GetRulesForProfileUsingRulesApp(ISonarConfiguration conf, Profile profile, bool active);

        /// <summary>
        /// Get rules using profile app id
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="ruleKey">rule key</param>
        /// <returns>rule</returns>
        Rule GetRuleUsingProfileAppId(ISonarConfiguration conf, string ruleKey);

        /// <summary>
        /// Gets duplicatations
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="resourceKey">resource key</param>
        /// <returns>dup data</returns>
        List<DuplicationData> GetDuplicationsDataInResource(ISonarConfiguration conf, string resourceKey);

        /// <summary>
        /// Ignores all file.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="project">The project.</param>
        /// <param name="file">The file.</param>
        /// <returns>returns a list of exclusions</returns>
        IList<Exclusion> IgnoreAllFile(ISonarConfiguration conf, Resource project, string file);

        /// <summary>
        /// Ignores the rule on file.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="project">The project.</param>
        /// <param name="file">The file.</param>
        /// <param name="rule">The rule.</param>
        /// <returns>returns a list of exclusions</returns>
        IList<Exclusion> IgnoreRuleOnFile(ISonarConfiguration conf, Resource project, string file, Rule rule);

        /// <summary>
        /// Gets the exclusions.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="project">The project.</param>
        /// <returns>returns a list of exclusions</returns>
        IList<Exclusion> GetExclusions(ISonarConfiguration conf, Resource project);

        /// <summary>
        /// Copies the profile.
        /// </summary>
        /// <param name="conf">conf</param>
        /// <param name="id">The identifier.</param>
        /// <param name="newName">The new name.</param>
        /// <returns></returns>
        string CopyProfile(ISonarConfiguration conf, string id, string newName);

        /// <summary>
        /// Deletes the profile.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="profileKey">The profile key.</param>
        /// <returns></returns>
        string DeleteProfile(ISonarConfiguration conf, string profileKey);

        /// <summary>
        /// Changes the parent profile.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="profileKey">The profile key.</param>
        /// <param name="parentKey">The parent key.</param>
        /// <returns></returns>
        string ChangeParentProfile(ISonarConfiguration conf, string profileKey, string parentKey);

        /// <summary>
        /// Gets the parent profile.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="profileKey">The profile key.</param>
        /// <returns></returns>
        string GetParentProfile(ISonarConfiguration conf, string profileKey);

        /// <summary>
        /// Assigns the profile to project.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="profileKey">The profile key.</param>
        /// <param name="projectKey">The project key.</param>
        /// <returns></returns>
        string AssignProfileToProject(ISonarConfiguration conf, string profileKey, string projectKey);

        /// <summary>
        /// Gets the installed plugins.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <returns>plugins</returns>
        Dictionary<string, string> GetInstalledPlugins(ISonarConfiguration conf);

        /// <summary>
        /// Cancels the request.
        /// </summary>
        void CancelRequest();

        /// <summary>
        /// Applies the permission template to project.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="projectKey">The project key.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>returns error message or empty if ok</returns>
        string ApplyPermissionTemplateToProject(ISonarConfiguration conf, string projectKey, string templateName);

        /// <summary>
        /// Gets the blame line.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="key">The key.</param>
        /// <param name="line">The line.</param>
        /// <param name="token">token</param>
        /// <param name="logger">logger</param>
        /// <returns>blame info</returns>
        Task<BlameLine> GetBlameLine(ISonarConfiguration conf, string key, int line, CancellationToken token, IRestLogger logger);

        /// <summary>
        /// Indexes the server resources.
        /// </summary>
        /// <param name="conf">The conf.</param>
        /// <param name="project">The project.</param>
        /// <param name="token">token</param>
        /// <param name="logger">logger</param>
        /// <returns>resources</returns>
        Task<List<Resource>> IndexServerResources(ISonarConfiguration conf, Resource project, CancellationToken token, IRestLogger logger);
    }
}