﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeProviders.fs" company="Copyright © 2014 Tekla Corporation. Tekla is a Trimble Company">
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

type CopyProfileAnswer = JsonProvider<""" {"key":"cs-profile2-77634","name":"profile2","language":"cs","languageName":"C#","isDefault":false,"isInherited":false} """>

type TemplateSearchAnswer = JsonProvider<""" {"permissionTemplates":[{"id":"AVGP9Kxascyut2XzfoAC","name":"thisisname","description":"this is description","projectKeyPattern":"KeyPattern:*","createdAt":"2015-12-11T09:32:35+0200","updatedAt":"2015-12-11T09:32:35+0200","permissions":[{"key":"admin","usersCount":0,"groupsCount":1},{"key":"codeviewer","usersCount":0,"groupsCount":2},{"key":"issueadmin","usersCount":0,"groupsCount":1},{"key":"user","usersCount":0,"groupsCount":2}]}],"defaultTemplates":[{"templateId":"default_template_for_projects","qualifier":"VW"},{"templateId":"default_template_for_projects","qualifier":"TRK"}],"permissions":[{"key":"user","name":"Browse","description":"Access a project, browse its measures, and create/edit issues for it."},{"key":"admin","name":"Administer","description":"Access project settings and perform administration tasks. (Users will also need \"Browse\" permission)"},{"key":"issueadmin","name":"Administer Issues","description":"Perform advanced editing on issues: marking an issue False Positive / Won't Fix, and changing an Issue's severity. (Users will also need \"Browse\" permission)"},{"key":"codeviewer","name":"See Source Code","description":"View the project's source code. (Users will also need \"Browse\" permission)"}]} """>

type ComponentSearchAnswer = JsonProvider<""" {"paging":{"pageIndex":1,"pageSize":100,"total":2},"components":[{"id":"0fce4202-4074-449f-a20b-ebe42cbb69f3","key":"Key:key","qualifier":"DIR","name":"name"},{"id":"de508632-5640-4a82-950a-d3fe0c5d7cd4","key":"project:Common:name/interface","qualifier":"DIR","name":"name/interface"}]} """>

type ScmAnswer = JsonProvider<"""
{
  "scm": [
    [1, "julien", "2013-03-13T12:34:56+0100", "a1e2b3e5d6f5"],
    [2, "julien", "2013-03-14T13:17:22+0100", "b1e2b3e5d6f5"],
    [3, "simon", "2014-01-01T15:35:36+0100", "c1e2b3e5d6f5"]
  ]
}
""">

type JsonarProfileInheritance = JsonProvider<""" {
  "profile": {
    "key": "xoo-my-bu-profile-23456",
    "name": "My BU Profile",
    "parent": "xoo-my-company-profile-12345",
    "activeRuleCount": 3,
    "overridingRuleCount": 1
  },
  "ancestors": [
    {
      "key": "xoo-my-company-profile-12345",
      "name": "My Company Profile",
      "parent": "xoo-my-group-profile-01234",
      "activeRuleCount": 3
    },
    {
      "key": "xoo-my-group-profile-01234",
      "name": "My Group Profile",
      "activeRuleCount": 2
    }
  ],
  "children": [
    {
      "key": "xoo-for-project-one-34567",
      "name": "For Project One",
      "activeRuleCount": 5
    },
    {
      "key": "xoo-for-project-two-45678",
      "name": "For Project Two",
      "activeRuleCount": 4,
      "overridingRuleCount": 1
    }
  ]
} """>

type JsonTags = JsonProvider<""" {
  "tags": [
    "naming",
    "unused-code",
    "pitfall",
    "convention",
    "security",
    "size",
    "error-handling",
    "multithreading",
    "bug",
    "unused",
    "java8",
    "brain-overload",
    "comment",
    "formatting"
  ]
} """>

type PluginsMessage = JsonProvider<""" {"plugins":[{"key":"csharp","name":"C#","description":"Enable analysis and reporting on C# projects.","version":"4.4-SNAPSHOT","license":"GNU LGPL 3","organizationName":"SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://redirect.sonarsource.com/plugins/csharp.html","issueTrackerUrl":"http://jira.codehaus.org/browse/SONARCS","implementationBuild":"de03f5e6ca851ecf2240651d94df9b2dc345e582"},{"key":"cxx","name":"C++ (Community)","description":"Enable analysis and reporting on c++ projects.","version":"0.9.5-SNAPSHOT","license":"GNU LGPL 3","organizationName":"Waleri Enns","homepageUrl":"https://github.com/SonarOpenCommunity/sonar-cxx/wiki","issueTrackerUrl":"https://github.com/SonarOpenCommunity/sonar-cxx/issues?state=open","implementationBuild":"0"},{"key":"fsharp","name":"F#","description":"Enable analysis and reporting on F# projects.","version":"1.0.RC1","license":"GNU LGPL 3","organizationName":"Jorge Costa and SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://redirect.sonarsource.com/plugins/fsharp.html","issueTrackerUrl":"https://github.com/jmecosta/sonar-fsharp-plugin/issues","implementationBuild":"1d151f79651235403dbf6d1b94e0f997b95619b2"},{"key":"scmgit","name":"Git","description":"Git SCM Provider.","version":"1.1","license":"GNU LGPL 3","organizationName":"SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://redirect.sonarsource.com/plugins/scmgit.html","issueTrackerUrl":"http://jira.codehaus.org/browse/SONARSCGIT","implementationBuild":"21e7329a632904350bb9a2e7f1b17b9967988db8"},{"key":"jira","name":"JIRA","description":"Connects SonarQube to Atlassian JIRA in various ways.","version":"1.2","license":"GNU LGPL 3","organizationName":"SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://docs.codehaus.org/display/SONAR/Jira+Plugin","issueTrackerUrl":"http://jira.codehaus.org/browse/SONARPLUGINS/component/13914","implementationBuild":"71e8002a5e7948ec705648d336e8bb9ab8026c55"},{"key":"java","name":"Java","description":"SonarQube rule engine.","version":"3.7.1","license":"GNU LGPL 3","organizationName":"SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://redirect.sonarsource.com/plugins/java.html","issueTrackerUrl":"http://jira.codehaus.org/browse/SONARJAVA","implementationBuild":"af982dcb9e04d3c0b8570185766b531e33b37948"},{"key":"javascript","name":"JavaScript","description":"Enables analysis of JavaScript projects.","version":"2.8","license":"GNU LGPL 3","organizationName":"SonarSource and Eriks Nukis","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://redirect.sonarsource.com/plugins/javascript.html","issueTrackerUrl":"https://jira.codehaus.org/browse/SONARJS","implementationBuild":"53ffb46f827d24be6173dc5a44afd74b2c0b4e3f"},{"key":"ldap","name":"LDAP","description":"Delegates authentication to LDAP","version":"1.5.1","license":"GNU LGPL 3","organizationName":"SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://redirect.sonarsource.com/plugins/ldap.html","issueTrackerUrl":"http://jira.sonarsource.com/browse/LDAP","implementationBuild":"8960e08512a3d3ec4d9cf16c4c2c95017b5b7ec5"},{"key":"motionchart","name":"Motion Chart","description":"Display how a set of metrics evolves over time (requires an internet access).","version":"1.7","license":"GNU LGPL 3","organizationName":"SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://docs.codehaus.org/display/SONAR/Motion+Chart+plugin","issueTrackerUrl":"http://jira.codehaus.org/browse/SONARPLUGINS/component/13722","implementationBuild":"e9c4a5c95c75564b3c3b5a887b63ef50fc59a156"},{"key":"python","name":"Python","description":"Enable analysis and reporting on python projects.","version":"1.5","license":"GNU LGPL 3","organizationName":"SonarSource and Waleri Enns","homepageUrl":"http://docs.codehaus.org/display/SONAR/Python+Plugin","issueTrackerUrl":"https://jira.codehaus.org/browse/SONARPY","implementationBuild":"10c8f1d2e8ded13634d3ee71c096e97d3fb3cfe9"},{"key":"roslyn","name":"Roslyn","description":"Roslyn diagnostic runner","version":"0.9-SNAPSHOT","license":"GNU LGPL 3","organizationName":"jmecsoftware.com","organizationUrl":"http://www.sonarsource.com","homepageUrl":"https://sites.google.com/site/jmecsoftware/","issueTrackerUrl":"http://jira.sonarsource.com","implementationBuild":"a72d5ce144164a9dda65ac4be277265194fa212c"},{"key":"scmsvn","name":"SVN","description":"SVN SCM Provider.","version":"1.2","license":"GNU LGPL 3","organizationName":"SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://redirect.sonarsource.com/plugins/scmsvn.html","issueTrackerUrl":"https://jira.sonarsource.com/browse/SONARSCSVN","implementationBuild":"d04c3cdb21f48905dd8300d1129ec90281aa6db2"},{"key":"stylecop","name":"StyleCop","description":"Enables the use of StyleCop rules on C# code.","version":"1.1","license":"GNU LGPL 3","organizationName":"SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://docs.codehaus.org/x/BoNEDg","issueTrackerUrl":"http://jira.codehaus.org/browse/SONARPLUGINS/component/16487","implementationBuild":"909438ebc609371919de34aa41262093711c58bc"},{"key":"timeline","name":"Timeline","description":"Advanced time machine chart (requires an internet access).","version":"1.5","license":"GNU LGPL 3","organizationName":"SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://docs.codehaus.org/display/SONAR/Timeline+Plugin","issueTrackerUrl":"http://jira.codehaus.org/browse/SONARPLUGINS/component/14068","implementationBuild":"a9cae1328fd455a128b5d7d603381f47398c6e2a"},{"key":"widgetlab","name":"Widget Lab","description":"Additional widgets","version":"1.7","license":"GNU LGPL 3","organizationName":"Shaw Industries","organizationUrl":"http://shawfloors.com","homepageUrl":"http://docs.codehaus.org/display/SONAR/Widget+Lab+Plugin","issueTrackerUrl":"http://jira.codehaus.org/browse/SONARPLUGINS/component/15490","implementationBuild":"199762d2ed62a215601f3422a4973682c16618c0"},{"key":"xml","name":"XML","description":"Enable analysis and reporting on XML files.","version":"1.3","license":"The Apache Software License, Version 2.0","organizationName":"SonarSource","organizationUrl":"http://www.sonarsource.com","homepageUrl":"http://redirect.sonarsource.com/plugins/xml.html","issueTrackerUrl":"http://jira.codehaus.org/browse/SONARPLUGINS/component/14607","implementationBuild":"a8739cf424a5b42b64a3277373ab2d48aca5a6e0"},{"key":"ail","name":"ail","description":"Enable analysis and reporting on ail files.","version":"0.9-SNAPSHOT","license":"GNU LGPL 3","organizationName":"Jorge Costa","organizationUrl":"http://www.tekla.com","homepageUrl":"http://redirect.sonarsource.com/plugins/fsharp.html","issueTrackerUrl":"http://jira.codehaus.org/browse/SONARCS","implementationBuild":"1931514c7e0cefee876b21aeea139f895a5fac79"}]}""">

type JsonErrorMessage = JsonProvider<""" {"errors":[{"msg":"Linear functions must only have a non empty coefficient"}]} """>

type JsonarRuleShowResponse = JsonProvider<""" {"rule":{"key":"xml:IllegalTabCheck","repo":"xml","name":"Tabulation characters should not be used","createdAt":"2016-09-10T09:42:07+0300","htmlDesc":"<p>\n  Developers should not need to configure the tab width of their text editors in order to be able to read source code.\n  So the use of tabulation character must be banned.\n</p>","mdDesc":"<p>\n  Developers should not need to configure the tab width of their text editors in order to be able to read source code.\n  So the use of tabulation character must be banned.\n</p>","severity":"MINOR","status":"READY","isTemplate":false,"tags":[],"sysTags":["convention"],"lang":"xml","langName":"XML","params":[{"key":"markAll","htmlDesc":"Mark all tab errors","defaultValue":"false","type":"BOOLEAN"}],"defaultDebtRemFnType":"CONSTANT_ISSUE","defaultDebtRemFnOffset":"2min","debtOverloaded":false,"debtRemFnType":"CONSTANT_ISSUE","debtRemFnOffset":"2min","defaultRemFnType":"CONSTANT_ISSUE","defaultRemFnBaseEffort":"2min","remFnType":"CONSTANT_ISSUE","remFnBaseEffort":"2min","remFnOverloaded":false,"type":"CODE_SMELL"},"actives":[{"qProfile":"xmlsss-sonar-way-85839","inherit":"NONE","severity":"MINOR","params":[{"key":"dummu","value":"false"},{"key":"dummy","value":"sdasa"}],"createdAt":"2016-09-10T09:42:16+0300"}]} """>


type JsonRuleSearchResponse = JsonProvider<""" {
  "total": 641,
  "p": 1,
  "ps": 10,
  "rules": [
    {
      "key": "cppcheck:unreadVariable",
      "repo": "cppcheck",
      "name": "Unused value",
      "createdAt": "2013-08-19T23:16:21.0.00",
      "severity": "MAJOR",
      "status": "READY",
      "internalKey": "unreadVariable",
      "isTemplate": false,
      "templateKey": "cxx:CommentRegularExpression",
      "tags": [
        "pitfall",
        "unused"
      ],
      "sysTags": [
        "pitfall",
        "unused"
      ],
      "lang": "c++",
      "langName": "c++",
      "htmlDesc": "Variable is assigned a value that is never used.",
      "defaultDebtChar": "RELIABILITY",
      "defaultDebtSubChar": "INSTRUCTION_RELIABILITY",
      "debtChar": "RELIABILITY",
      "debtSubChar": "INSTRUCTION_RELIABILITY",
      "debtCharName": "Reliability",
      "debtSubCharName": "Instruction",
      "defaultDebtRemFnType": "LINEAR",
      "defaultDebtRemFnCoeff": "5min",
      "debtOverloaded": false,
      "debtRemFnType": "LINEAR",
      "debtRemFnCoeff": "5min",
      "params": [],
      "type" : "CODE_SMELL"
    },
    {
      "type" : "CODE_SMELL",
      "key": "cppcheck:arrayIndexOutOfBounds",
      "repo": "cppcheck",
      "name": "Array index out of bounds",
      "createdAt": "2013-08-19T23:16:21.0.00",
      "severity": "MAJOR",
      "status": "READY",
      "internalKey": "arrayIndexOutOfBounds",
      "isTemplate": false,
      "tags": [],
      "sysTags": [],
      "lang": "c++",
      "langName": "c++",
      "htmlDesc": "Array index out of bounds.",
      "defaultDebtChar": "RELIABILITY",
      "defaultDebtSubChar": "INSTRUCTION_RELIABILITY",
      "debtChar": "RELIABILITY",
      "debtSubChar": "INSTRUCTION_RELIABILITY",
      "debtCharName": "Reliability",
      "debtSubCharName": "Instruction",
      "defaultDebtRemFnType": "LINEAR",
      "defaultDebtRemFnCoeff": "30min",
      "debtOverloaded": false,
      "debtRemFnType": "LINEAR",
      "debtRemFnCoeff": "30min",
      "params": [
        {
          "key": "CheckId",
          "type": "STRING",
          "defaultValue": "TE0027",
          "htmlDesc": "description"
        }
      ]
      }
    ],
      "facets": [
        {
           "property": "debt_characteristics",
           "values": [
             {
                "val" : "ARCHITECTURE_CHANGEABILITY", "count":1
             },
             {
                "val":"CHANGEABILITY","count":1
             },
             {
                "val":"NONE","count":0
             }]
         }
        ]
} """>

type JsonInternalData = JsonProvider<""" {
  "canWrite": true,
  "qualityprofiles": [
    {
      "key": "cs-default-tekla-c-84184",
      "name": "Default Tekla C#",
      "lang": "cs"
    },
    {
      "key": "c++-defaultc++reinforcement-41625",
      "name": "DefaultC++Reinforcement",
      "lang": "c++"
    }
  ],
  "languages": {
    "py": "Python",
    "c++": "c++",
    "xaml": "xaml",
    "cs": "C#"
  },
  "repositories": [
    {
      "key": "checkstyle",
      "name": "Checkstyle",
      "language": "java"
    },
    {
      "key": "common-c++",
      "name": "Common SonarQube",
      "language": "c++"
    }
  ],
  "statuses": {
    "BETA": "Beta",
    "DEPRECATED": "Deprecated",
    "READY": "Ready"
  },
  "characteristics": {
    "INTEGRATION_TESTABILITY": "Testability: Integration level",
    "UNIT_TESTABILITY": "Testability: Unit level",
    "REUSABILITY": "Reusability",
    "COMPILER_RELATED_PORTABILITY": "Portability: Compiler",
    "PORTABILITY": "Portability",
    "TRANSPORTABILITY": "Reusability: Transportability",
    "MODULARITY": "Reusability: Modularity",
    "SECURITY": "Security",
    "API_ABUSE": "Security: API abuse",
    "ERRORS": "Security: Errors",
    "INPUT_VALIDATION_AND_REPRESENTATION": "Security: Input validation and representation",
    "SECURITY_FEATURES": "Security: Security features",
    "EFFICIENCY": "Efficiency",
    "MEMORY_EFFICIENCY": "Efficiency: Memory use",
    "NETWORK_USE": "Efficiency: Network use",
    "HARDWARE_RELATED_PORTABILITY": "Portability: Hardware",
    "LANGUAGE_RELATED_PORTABILITY": "Portability: Language",
    "OS_RELATED_PORTABILITY": "Portability: OS",
    "SOFTWARE_RELATED_PORTABILITY": "Portability: Software",
    "TIME_ZONE_RELATED_PORTABILITY": "Portability: Time zone",
    "MAINTAINABILITY": "Maintainability",
    "READABILITY": "Maintainability: Readability",
    "UNDERSTANDABILITY": "Maintainability: Understandability",
    "FAULT_TOLERANCE": "Reliability: Fault tolerance",
    "EXCEPTION_HANDLING": "Reliability: Exception handling",
    "LOGIC_RELIABILITY": "Reliability: Logic",
    "INSTRUCTION_RELIABILITY": "Reliability: Instruction",
    "SYNCHRONIZATION_RELIABILITY": "Reliability: Synchronization",
    "RESOURCE_RELIABILITY": "Reliability: Resource",
    "TESTABILITY": "Testability",
    "UNIT_TESTS": "Reliability: Unit tests coverage",
    "CHANGEABILITY": "Changeability",
    "CPU_EFFICIENCY": "Efficiency: Processor use",
    "DATA_CHANGEABILITY": "Changeability: Data",
    "ARCHITECTURE_CHANGEABILITY": "Changeability: Architecture",
    "RELIABILITY": "Reliability",
    "LOGIC_CHANGEABILITY": "Changeability: Logic",
    "DATA_RELIABILITY": "Reliability: Data",
    "ARCHITECTURE_RELIABILITY": "Reliability: Architecture"
  }
}""" >

type JsonRule = JsonProvider<""" {"rule": {
    "key":"cppcheck:unreadVariable",
    "repo":"cppcheck",
    "name":"Unused value",
    "createdAt":"2013-08-19T23:16:21.0.00",
    "severity":"MAJOR",
    "status":"READY",
    "internalKey":"unreadVariable",
    "isTemplate":false,
    "tags":[
      "pitfall",
      "unused"
    ],
    "sysTags":[
      "pitfall",
      "unused"
    ],
    "lang":"c++",
    "langName":"c++",
    "htmlDesc":"Variable is assigned a value that is never used.",
    "defaultDebtChar":"RELIABILITY",
    "defaultDebtSubChar":"INSTRUCTION_RELIABILITY",
    "debtChar":"RELIABILITY",
    "debtSubChar":"INSTRUCTION_RELIABILITY",
    "debtCharName":"Reliability",
    "debtSubCharName":"Instruction",
    "defaultDebtRemFnType":"LINEAR",
    "defaultDebtRemFnCoeff":"5min",
    "debtOverloaded":false,
    "debtRemFnType":"LINEAR",
    "debtRemFnCoeff":"5min",
    "params": [
        {
            "key": "max",
            "desc": "Maximum complexity allowed.",
            "defaultValue": "200"
        }
    ] 
}, "actives": [
    {
        "qProfile": "Sonar way with Findbugs:java",
        "inherit": "NONE",
        "severity": "MAJOR",
        "params": [
            {
                "key": "max",
                "value": "200"
            }
        ]
    },
    {
        "qProfile": "Sonar way:java",
        "inherit": "NONE",
        "severity": "MAJOR",
        "params": [
            {
                "key": "max",
                "value": "200"
            }
        ]
    }
]} """>

type JsonProjectIndex = JsonProvider<""" [
  {
    "id": "5035",
    "k": "org.jenkins-ci.plugins:sonar",
    "nm": "Jenkins Sonar Plugin",
    "sc": "PRJ",
    "qu": "TRK"
  },
  {
    "id": "5146",
    "k": "org.codehaus.sonar-plugins:sonar-ant-task",
    "nm": "Sonar Ant Task",
    "sc": "PRJ",
    "qu": "TRK"
  },
  {
    "id": "15964",
    "k": "org.codehaus.sonar-plugins:sonar-build-breaker-plugin",
    "nm": "Sonar Build Breaker Plugin",
    "sc": "PRJ",
    "qu": "TRK"
  }
] """>

type JsonQualityProfiles = JsonProvider<""" [
  {
    "name": "Sonar way with Findbugs",
    "language": "java",
    "default": false
  },
  {
    "name": "Sonar way",
    "language": "java",
    "default": false
  }
] """>

type JsonQualityProfiles80 = JsonProvider<"""
  {
     "profiles":[
        {
           "key":"xml-sonar-way-00258",
           "name":"Sonar way (outdated copy)",
           "language":"xml",
           "languageName":"XML",
           "isInherited":false,
           "isDefault":true,
           "activeRuleCount":6,
           "activeDeprecatedRuleCount":0,
           "rulesUpdatedAt":"2022-02-13T09:02:30+0000",
           "lastUsed":"2022-02-18T15:37:33+0200",
           "userUpdatedAt":"2022-02-13T11:02:30+0200",
           "isBuiltIn":false,
           "actions":{
              "edit":true,
              "setAsDefault":false,
              "copy":true,
              "associateProjects":false,
              "delete":false
           }
        },
        {
           "key":"AV5TE3CkFrY769DgAO8u",
           "name":"TCD",
           "language":"xml",
           "languageName":"XML",
           "isInherited":false,
           "isDefault":false,
           "activeRuleCount":6,
           "activeDeprecatedRuleCount":0,
           "projectCount":1,
           "rulesUpdatedAt":"2022-02-13T09:02:31+0000",
           "lastUsed":"2022-02-17T14:35:24+0200",
           "userUpdatedAt":"2022-02-13T11:02:31+0200",
           "isBuiltIn":false,
           "actions":{
              "edit":true,
              "setAsDefault":true,
              "copy":true,
              "associateProjects":true,
              "delete":true
           }
        }
     ],
     "actions":{
        "create":true
     }
  }
 """>

type JsonProfileAfter44 = JsonProvider<""" [
  {
    "name": "Sonar way",
    "language": "java",
    "default": true,
    "rules": [
      {
        "key": "DuplicatedBlocks",
        "repo": "common-java",
        "severity": "MAJOR"
      },
      {
        "key": "InsufficientBranchCoverage",
        "repo": "common-java",
        "severity": "MAJOR",
        "params": [
          {
            "key": "minimumBranchCoverageRatio",
            "value": "65.0"
          }
        ]
      },
      {
        "key": "S00105",
        "repo": "squid",
        "severity": "MINOR"
      },
      {
        "key": "MethodCyclomaticComplexity",
        "repo": "squid",
        "severity": "MAJOR",
        "params": [
          {
            "key": "sdfsdfsd",
            "value": "sdfsdfsd"
          }
        ]
      }
    ]
  }
] """>

type JsonComponentShow = JsonProvider<""" {"component":{"organization":"default-organization","id":"AVdhu8i3reqscUIhfy2g","key":"org:prj:file.cpp","name":"file.cpp","qualifier":"FIL","path":"file.cpp","language":"c++"},"ancestors":[{"organization":"default-organization","id":"AVdhu8i4reqscUIhfy28","key":"org:proj:lib","name":"lib","qualifier":"DIR","path":"libprofdb"},{"organization":"default-organization","id":"AVdhu8OxiJrVXuW77K-o","key":"org:prj","name":"proj","qualifier":"TRK"}]} """>
type JsonQualityProfiles63 = JsonProvider<""" {"profiles":[{"key":"ail-sonar-way-92014","name":"Sonar way","language":"ail","languageName":"AIL","isInherited":false,"isDefault":true,"activeRuleCount":1,"activeDeprecatedRuleCount":0,"rulesUpdatedAt":"2016-09-10T06:42:16+0000","lastUsed":"2016-10-25T21:55:27+0300"},{"key":"c++-defaultteklac++-75560","name":"DefaultTeklaC++","language":"c++","languageName":"c++","isInherited":false,"isDefault":true,"activeRuleCount":1386,"activeDeprecatedRuleCount":0,"rulesUpdatedAt":"2016-09-10T06:55:42+0000","lastUsed":"2016-10-25T21:55:41+0300","userUpdatedAt":"2016-09-10T09:55:42+0300"},{"key":"cs-default-tekla-c-roslyn-76974","name":"Default Tekla C# - Roslyn","language":"cs","languageName":"C#","isInherited":false,"isDefault":true,"activeRuleCount":266,"activeDeprecatedRuleCount":0,"rulesUpdatedAt":"2016-09-26T18:56:30+0000","lastUsed":"2016-11-11T18:15:09+0200","userUpdatedAt":"2016-09-26T21:56:30+0300"},{"key":"fs-sonar-way-83735","name":"Sonar way","language":"fs","languageName":"F#","isInherited":false,"isDefault":true,"activeRuleCount":38,"activeDeprecatedRuleCount":0,"rulesUpdatedAt":"2016-09-10T06:42:16+0000","lastUsed":"2016-11-11T18:15:09+0200"},{"key":"java-sonar-way-02421","name":"Sonar way","language":"java","languageName":"Java","isInherited":false,"isDefault":true,"activeRuleCount":266,"activeDeprecatedRuleCount":0,"rulesUpdatedAt":"2016-09-10T06:42:16+0000"},{"key":"js-sonar-way-94233","name":"Sonar way","language":"js","languageName":"JavaScript","isInherited":false,"isDefault":true,"activeRuleCount":89,"activeDeprecatedRuleCount":1,"rulesUpdatedAt":"2016-09-10T06:42:16+0000"},{"key":"msbuild-default-msbuild-profile-42378","name":"Default MSBuild Profile","language":"msbuild","languageName":"MSBuild","isInherited":false,"isDefault":true,"activeRuleCount":30,"activeDeprecatedRuleCount":0,"rulesUpdatedAt":"2016-09-26T06:54:03+0000","lastUsed":"2016-11-11T18:15:09+0200","userUpdatedAt":"2016-09-26T09:54:03+0300"},{"key":"py-sonar-way-65505","name":"Sonar way","language":"py","languageName":"Python","isInherited":false,"isDefault":true,"activeRuleCount":35,"activeDeprecatedRuleCount":0,"rulesUpdatedAt":"2016-09-10T06:42:16+0000"},{"key":"xml-sonar-way-85839","name":"Sonar way","language":"xml","languageName":"XML","isInherited":false,"isDefault":true,"activeRuleCount":4,"activeDeprecatedRuleCount":0,"rulesUpdatedAt":"2016-09-10T06:42:16+0000","lastUsed":"2016-11-11T18:15:09+0200"}]} """>
type JsonComponents = JsonProvider<""" {"paging":{"pageIndex":1,"pageSize":100,"total":15},"components":[{"organization":"default-organization","id":"AVdiEzKfrCEH-mFKGDOE","key":"Org2.core:Analysis","name":"Analysis","qualifier":"TRK"},{"organization":"default-organization","id":"AVdho8PGiJrVXuW77K-i","key":"Org2.Tools.BuildAllExtension","name":"BuildAllExtension","qualifier":"TRK"},{"organization":"default-organization","id":"AVdhu8OxiJrVXuW77K-o","key":"Org2.core:Catalogs","name":"Catalogs","qualifier":"TRK"},{"organization":"default-organization","id":"AVcTD_XvCqG-VILNsN8K","key":"Org2.core:Common","name":"Common","qualifier":"TRK"},{"organization":"default-organization","id":"AVdhsdkhiJrVXuW77K-m","key":"Org2.core:Dimensioning","name":"Dimensioning","qualifier":"TRK"},{"organization":"default-organization","id":"AVdlhcSgrCEH-mFKGDOS","key":"Org2.core:Drawings","name":"Drawings","qualifier":"TRK"},{"organization":"default-organization","id":"AVdjXA7zrCEH-mFKGDON","key":"Org2.core:DrawingTester","name":"DrawingTester","qualifier":"TRK"},{"organization":"default-organization","id":"AVdhxtAuiJrVXuW77K-q","key":"Org2.core:Environment","name":"Environment","qualifier":"TRK"},{"organization":"default-organization","id":"AVdlmdD2rCEH-mFKGDOU","key":"Org2.core:Model","name":"Model","qualifier":"TRK"},{"organization":"default-organization","id":"AVdjM-7KrCEH-mFKGDOG","key":"Org2.core:ModelTester","name":"ModelTester","qualifier":"TRK"},{"organization":"default-organization","id":"AVdhx_VWiJrVXuW77K-s","key":"Org2.core:Reinforcement","name":"Reinforcement","qualifier":"TRK"},{"organization":"default-organization","id":"AVdn2VqWrCEH-mFKGDQY","key":"Org.Connect.Desktop:feature_TCD-991_SlideShow-2","name":"Org.Connect.Desktop feature_TCD-991_SlideShow-2","qualifier":"TRK"},{"organization":"default-organization","id":"AVdnuwwzrCEH-mFKGDOW","key":"Org.Connect.Desktop:master","name":"Org.Connect.Desktop master","qualifier":"TRK"}]} """>

type JsonResourceWithMetrics = JsonProvider<""" [{"id":1,"key":"GroupId:ProjectId","name":"Common","scope":"PRJ","branch":"whatever","qualifier":"TRK","date":"2013-07-03T12:50:52+0300","lname":"Common","lang":"c++","version":"work","description":"","msr":[{"key":"ncloc","val":45499.0,"frmt_val":"45,499"},{"key":"coverage","val":54.7,"frmt_val":"54.7%"},{"key":"profile","val":7.0,"frmt_val":"7.0","data":"DefaultTeklaC++"}]}] """>

type JsonValidateUser = JsonProvider<""" {"valid":true} """>

type JSonProfile = JsonProvider<""" [{"name":"profile","language":"lang","default":true,"rules":[{"key":"key1","repo":"repo1","severity":"BLOCKER"},{"key":"key2","repo":"repo","severity":"CRITICAL"}],"alerts":[{"metric":"metric1","operator":">","error":"50","warning":"70"},{"metric":"metric1","operator":">","error":"50","warning":"70"}]}] """>

type JSonServerInfo = JsonProvider<""" {"id":"20130712144608","version":"3.6.1-SNAPSHOT","status":"UP"} """>

type JSonSource = JsonProvider<""" [{"line1":"/**","line2":"    @file       bla.cpp","line3":"    Source file of the bla class.","line4":"    @author     bla","line5":"    @date       25.11.2002","line6":"*/"}] """>

type JSonComment = JsonProvider<""" {"comment":{"key":"xfasdegfdfDffd","login":"login1","htmlText":"fsdfsd","createdAt":"2013-07-17T23:46:44+0300"}} """>

type JSonProperties = JsonProvider<""" [{"key":"sonar.permission.template.default","value":"default_template_for_projects"},{"key":"sonar.cpd.cross_project","value":"true"},{"key":"sonar.dbcleaner.weeksBeforeDeletingAllSnapshots","value":"6000"},{"key":"sonar.fxcop.installDirectory","value":"C:\\\\Program Files (x86)\\\\Microsoft Fxcop 10.0"},{"key":"sonar.email.enabled","value":"true"},{"key":"sonar.dp.scm.command","value":"git log --numstat --date=iso"},{"key":"devcockpit.analysisDelayingInMinutes","value":"0"},{"key":"sonar.timeline.defaultmetrics","value":"lines,violations,coverage"},{"key":"sonar.branding.link","value":"http://www.tekla.com"},{"key":"sonar.branding.image","value":"http://www.tekla.com/_layouts/Tekla/images/logo.gif"},{"key":"sonar.branding.logo.location","value":"TOP"},{"key":"sonar.doxygenProperties.path","value":"e:\\\\sonar\\\\scripts\\\\doxygen.properties"},{"key":"sonar.forceAuthentication","value":"false"},{"key":"report.custom.plugins","value":"TQ, TDEBT, QI, SIGMM, TAGLIST"},{"key":"sonar.pdf.password","value":"username1"},{"key":"report.timeline.metrics","value":"loc, coverage, coverage_line_hits_data"},{"key":"report.delta.days","value":"300"},{"key":"sonar.pdf.username","value":"username1"},{"key":"send.email.to","value":"jmecosta@gmail.com"},{"key":"sonar.devcockpit.license.developers","value":"325435d2d4dfada9b7e78c5de7380b435a8e"},{"key":"sonar.cxx.suffixes","value":"c,cxx,cpp,cc,h,hxx,hpp,hh"},{"key":"sonar.core.projectsdashboard.showTreemap","value":"true"},{"key":"sonar.doxygen.path","value":"C:\\\\doxygen\\\\"},{"key":"sonar.pdf.skip","value":"true"},{"key":"vssonarextension.keys.138342495524701.vssonarextension.license.id","value":"<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<license id=\"b3f8d666-69e0-446f-8f19-26f7d910328c\" expiration=\"2014-12-12T00:00:00.0000000\" type=\"Standard\" ServerId=\"dsd6661ca19e7d\">\n  <name>BlaBla</name>\n  <Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\">\n    <SignedInfo>\n      <CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" />\n      <SignatureMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#rsa-sha1\" />\n      <Reference URI=\"\">\n        <Transforms>\n          <Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" />\n        </Transforms>\n        <DigestMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#sha1\" />\n        <DigestValue>io7viqk8/YhrSJdKiGjzP9q6sIg=</DigestValue>\n      </Reference>\n    </SignedInfo>\n    <SignatureValue>YbgAz8I69MVRLup3TZ6jSY4x5kranazEoAzsRzhdPorqFf8SrVDCzTfeR+thpv9EnP2OrqDmn+cTCkN1stuJEmnCvzEMo8VgomSyS66B5snkwQFO2DOjamUZl9mo8TxY15pwoULD7ejCpmgx3X1gIi+02BxKvfvL4wsbciaDUwA=</SignatureValue>\n  </Signature>\n</license>"},{"key":"vssonarextension.keys.138342495524701.vssonarextension.user.id","value":"dsadjks"},{"key":"vssonarextension.keys.138342567310101.vssonarextension.license.id","value":"dsjhjfdh"},{"key":"email.from","value":"sonar@tekla.com"},{"key":"email.prefix","value":"[SONAR]"},{"key":"sonar.doxygen.deploymentUrl","value":"http://sonar"},{"key":"sonar.doxygen.deploymentPath","value":"C:\\\\sonar-3.2\\\\war\\\\sonar-server"},{"key":"org.sonar.plugins.piwik.website-id","value":"3"},{"key":"sonar.scm.enabled","value":"false"},{"key":"sonar.core.serverBaseURL","value":"http://sonar:80"},{"key":"sonar.timemachine.period3","value":"previous_version"},{"key":"sonar.timemachine.period2","value":"5"},{"key":"com.tocea.scertifyRefactoringAssessment.projectNameEncryption","value":"true"},{"key":"sonar.profile.c","value":"Sonar way"},{"key":"sonar.profile.java","value":"Sonar way"},{"key":"sonar.profile.c++","value":"DefaultTeklaC++"},{"key":"sonar.core.projectsdashboard.defaultSortedColumn","value":"violations"},{"key":"sonar.dbcleaner.cleanDirectory","value":"false"},{"key":"sonar.profile.cs","value":"Default Tekla C#"},{"key":"sonar.profile.xaml","value":"TeklaCopXaml"},{"key":"sonar.organisation","value":"BlaBla"},{"key":"org.sonar.plugins.piwik.server","value":"esx-sonar:8000"},{"key":"sonar.server_id.ip_address","value":"10.42.65.244"},{"key":"sonar.server_id","value":"19fd6661ca19e7d"},{"key":"com.tocea.scertifyRefactoringAssessment.contactEmail","value":"jmecosta@gmail.com"},{"key":"sonar.profile.vbnet","value":"Sonar way"},{"key":"sonar.defaultGroup","value":"NewRegisteredUsers"},{"key":"sonar.switchoffviolations.multicriteria.135996266268601.resourceKey","value":"**/test/**"},{"key":"sonar.switchoffviolations.multicriteria.135996266268601.ruleKey","value":"cxxcustom:cpplint.readability/casting-0"},{"key":"sonar.global.exclusions","value":"AssemblyInfo.cs,Properties/AssemblyInfo.cs,**/*.ipch,**/_ReSharper.*/**,**/**/*.rc,**/**/resource.h,libcommondbase/v_*.h,libxml/**/**,**/**/*.bsc,file:**/libgr_plotdip/d_plotdev.c,file:**/libgr_plotdip/v_**plotdev.h,file:**/xs_cancel/dakbind.c,file:**/xs_cancel/dakbind.h,file:**/dllcom_analysis/analysisoptimisation_i.c,file:**/ail/dakbind.c,ail/dakbind.h,libenvdb_tables/Interface/v_*.h,AssemblyInfo.cpp","values":["AssemblyInfo.cs","Properties/AssemblyInfo.cs","**/*.ipch","**/_ReSharper.*/**","**/**/*.rc","**/**/resource.h","libcommondbase/v_*.h","libxml/**/**","**/**/*.bsc","file:**/libgr_plotdip/d_plotdev.c","file:**/libgr_plotdip/v_**plotdev.h","file:**/xs_cancel/dakbind.c","file:**/xs_cancel/dakbind.h","file:**/dllcom_analysis/analysisoptimisation_i.c","file:**/ail/dakbind.c","ail/dakbind.h","libenvdb_tables/Interface/v_*.h","AssemblyInfo.cpp"]},{"key":"sonar.global.test.exclusions","value":"Properties/AssemblyInfo.cs,AssemblyInfo.cpp","values":["Properties/AssemblyInfo.cs","AssemblyInfo.cpp"]},{"key":"sonar.switchoffviolations.multicriteria.135996266268601.lineRange","value":"*"},{"key":"sonar.core.projectsdashboard.columns","value":"METRIC.violations_density;METRIC.violations;METRIC.it_coverage;METRIC.it_line_coverage;METRIC.it_branch_coverage;METRIC.complexity;METRIC.line_coverage;METRIC.branch_coverage;"},{"key":"devcockpit.status","value":"D,20130819T16:59:27+0000"},{"key":"tendency.depth","value":"15"},{"key":"sonar.switchoffviolations.multicriteria","value":"135996266268601","values":["135996266268601"]},{"key":"sonar.allowUsersToSignUp","value":"true"},{"key":"sonar.permission.template.TRK.default","value":"default_template_for_projects"},{"key":"sonar.permission.template.DEV.default","value":"default_template_for_developers"},{"key":"sonar.core.id","value":"20131102223837"},{"key":"sonar.core.version","value":"3.7"},{"key":"sonar.core.startTime","value":"2013-11-02T22:38:37+0200"},{"key":"vssonarextension.keys","value":"138342495524701,138342567310101","values":["138342495524701","138342567310101"]}] """>

type JSonRule = JsonProvider<""" [{"title":"title of rule","key":"rulekey","plugin":"cxxexternal","config_key":"configKey","description":"this is the description","priority":"MAJOR"},{"title":"this is the title","key":"rulekey","plugin":"cxxexternal","config_key":"ruleconfigkey","description":"rule description","priority":"MAJOR"}] """>


type JSonDuplications = JsonProvider<""" [{"id":54245,"key":"groupId:ModuleId:ModelFile.cs","name":"ModelFile.cs","scope":"FIL","qualifier":"FIL","date":"2014-01-08T12:45:41+0200","creationDate":null,"lname":"ModelFile.cs","lang":"cs","msr":[{"key":"duplications_data","data":"<duplications><g><b s=\"439\" l=\"43\" r=\"groupId:ModuleId:file4.cs\"/><b s=\"499\" l=\"43\" r=\"groupId:ModuleId:file5.cs\"/></g></duplications>"}]},{"id":54595,"key":"groupId:ModuleId2:MeasurementGraphics/Providers/MeasurementGraphicsPointFace.cs","name":"MeasurementGraphicsPointFace.cs","scope":"FIL","qualifier":"FIL","date":"2014-01-08T12:45:41+0200","creationDate":null,"lname":"MeasurementGraphics/Providers/file1.cs","lang":"cs","msr":[{"key":"duplications_data","data":"<duplications><g><b s=\"141\" l=\"64\" r=\"groupId:ModuleId2:MeasurementGraphics/Providers/MeasurementGraphicsEdgeFace.cs\"/><b s=\"109\" l=\"64\" r=\"groupId:ModuleId2:MeasurementGraphics/Providers/MeasurementGraphicsPointFace.cs\"/></g></duplications>"}]},{"id":54232,"key":"groupId:ModuleId:Distance.cs","name":"Distance.cs","scope":"FIL","qualifier":"FIL","date":"2014-01-08T12:45:41+0200","creationDate":null,"lname":"Distance.cs","lang":"cs","msr":[{"key":"duplications_data","data":"<duplications><g><b s=\"825\" l=\"248\" r=\"groupId:ModuleId:Distance.cs\"/><b s=\"878\" l=\"216\" r=\"groupdId2:ModuleId3:Distance.cs\"/></g><g><b s=\"2250\" l=\"85\" r=\"groupId:ModuleId:Distance.cs\"/><b s=\"1308\" l=\"68\" r=\"groupdId2:ModuleId3:Distance.cs\"/></g><g><b s=\"740\" l=\"34\" r=\"groupId:ModuleId:Distance.cs\"/><b s=\"658\" l=\"34\" r=\"groupdId2:ModuleId3:Distance.cs\"/></g><g><b s=\"1822\" l=\"28\" r=\"groupId:ModuleId:Distance.cs\"/><b s=\"544\" l=\"28\" r=\"groupdId2:ModuleId3:Distance.cs\"/></g><g><b s=\"616\" l=\"28\" r=\"groupId:ModuleId:Distance.cs\"/><b s=\"352\" l=\"28\" r=\"groupdId2:ModuleId3:Distance.cs\"/></g><g><b s=\"2219\" l=\"19\" r=\"groupId:ModuleId:Distance.cs\"/><b s=\"1279\" l=\"19\" r=\"groupdId2:ModuleId3:Distance.cs\"/></g></duplications>"}]}] """>
