﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="Copyright © 2014 Tekla Corporation. Tekla is a Trimble Company">
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
    /// The user.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.AddionalEmails = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="active">
        /// The active.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="team">team</param>
        public User(string login, string name, bool active, string email, string team = "")
        {
            this.Login = login;
            this.Name = name;
            this.Active = active;
            this.Email = email;
            this.Team = team;
            this.AddionalEmails = new List<string>();
        }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Team
        /// </summary>
        public string Team { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="User"/> is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected; otherwise, <c>false</c>.
        /// </value>
        public bool Selected { get; set; }

        /// <summary>
        /// Addional emails alias if they exist
        /// </summary>
        public List<string> AddionalEmails { get; set; }

        private string DebuggerDisplay
        {
            get
            {
                return $"{Name} : {Email} => {string.Concat(AddionalEmails, ",")}";
            }
        }
    }
}
