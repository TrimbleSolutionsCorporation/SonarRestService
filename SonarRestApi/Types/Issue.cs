// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Issue.cs" company="Copyright © 2014 Tekla Corporation. Tekla is a Trimble Company">
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
namespace SonarRestService.Types
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// The issue.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Issue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Issue"/> class.
        /// </summary>
        public Issue()
        {
            Tags = new SortedSet<string>();
            Comments = new List<Comment>();
            Explanation = new List<ExplanationLine>();
            Message = string.Empty;
            CreationDate = DateTime.Now;
            CloseDate = DateTime.Now;
            Component = string.Empty;

            Line = -1;
            Project = string.Empty;
            UpdateDate = DateTime.Now;
            Status = IssueStatus.UNDEFINED;
            Severity = Severity.UNDEFINED;
            Rule = string.Empty;


            Resolution = Resolution.UNDEFINED;
            Assignee = string.Empty;
            Team = string.Empty;
            Author = string.Empty;
            IsNew = false;
            Key = string.Empty;

            Id = 0;
            ViolationId = 0;
        }

        /// <summary>Gets or sets the path.</summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the close date.
        /// </summary>
        public DateTime CloseDate { get; set; }

        /// <summary>
        /// Gets or sets the component.
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// Gets or sets the line.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Gets or set the text range
        /// </summary>
        public TextRange TextRange { get; set; }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>
        /// The column.
        /// </value>
        public int Column { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public IssueStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        public Severity Severity { get; set; }

        /// <summary>
        /// Gets or sets the rule.
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// Gets or sets the resolution.
        /// </summary>
        public Resolution Resolution { get; set; }

        /// <summary>
        /// Gets or sets the assignee.
        /// </summary>
        public string Assignee { get; set; }

        /// <summary>
        /// Team field
        /// </summary>
        public string Team { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is new.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is new; otherwise, <c>false</c>.
        /// </value>
        public bool IsNew { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the id, versious before 3.6
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the violation id.
        /// </summary>
        public int ViolationId { get; set; }

        /// <summary>
        /// Gets or sets the issue tracker identifier.
        /// </summary>
        /// <value>
        /// The issue tracker identifier.
        /// </value>
        public string IssueTrackerId { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the explanation.
        /// </summary>
        /// <value>
        /// The explanation.
        /// </value>
        public List<ExplanationLine> Explanation { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public SortedSet<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the effort.
        /// </summary>
        /// <value>
        /// The effort.
        /// </value>
        public string Effort { get; set; }

        /// <summary>
        /// Gets or sets the help URL.
        /// </summary>
        /// <value>
        /// The help URL.
        /// </value>
        public string HelpUrl { get; set; }

        /// <summary>
        /// The deep copy.
        /// </summary>
        /// <returns>
        /// The <see cref="Issue"/>.
        /// </returns>
        public Issue DeepCopy()
        {
            var copyIssue = new Issue
            {
                Id = Id,
                Key = Key,
                Line = Line,
                Message = Message,
                Project = Project,
                Resolution = Resolution,
                Rule = Rule,
                Severity = Severity,
                Status = Status,
                UpdateDate = UpdateDate,
                CloseDate = CloseDate,
                Component = Component,
                CreationDate = CreationDate,
                Effort = Effort,
                ViolationId = ViolationId,
                Assignee = Assignee,
                IssueTrackerId = IssueTrackerId,
                Comments = new List<Comment>()
            };

            foreach (var comment in Comments)
            {
                var copyComment = new Comment
                {
                    CreatedAt = comment.CreatedAt,
                    HtmlText = comment.HtmlText,
                    Id = comment.Id,
                    Key = comment.Key,
                    Login = comment.Login
                };
                copyIssue.Comments.Add(copyComment);
            }

            return copyIssue;
        }

        private string DebuggerDisplay => $"[{Line}:{Component}:{Key}] : {Status} :  {Author} : {Message}";
    }
}