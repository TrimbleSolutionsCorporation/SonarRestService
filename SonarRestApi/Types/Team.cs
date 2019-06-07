namespace SonarRestService.Types
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	/// <summary>
	/// Team
	/// </summary>
	public class Team
	{
		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Users
		/// </summary>
		public ICollection<User> Users { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="User"/> is selected.
		/// </summary>
		/// <value>
		///   <c>true</c> if selected; otherwise, <c>false</c>.
		/// </value>
		public bool Selected { get; set; }
	}
}
