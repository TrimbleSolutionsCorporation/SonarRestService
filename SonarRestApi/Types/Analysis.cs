namespace Trimble.SonarRestApi.Types
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Data transfer object for pagination information.
    /// Contains information about page size, current page index, and total items count.
    /// </summary>
    public class Paging
    {
        /// <summary>
        /// Gets or sets the current page index (1-based).
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int Total { get; set; }
    }

    /// <summary>
    /// Data transfer object for SonarQube analysis events.
    /// Events represent significant changes during an analysis like version changes or quality profile changes.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Gets or sets the unique identifier key for the event.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the event category (e.g., "VERSION", "QUALITY_PROFILE", "QUALITY_GATE").
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the display name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// dscription of the event.
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Data transfer object for SonarQube analysis information.
    /// Represents a single analysis of a project in SonarQube.
    /// </summary>
    public class Analysis
    {
        /// <summary>
        /// Gets or sets the unique identifier key for the analysis.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the date when the analysis was performed.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the list of events associated with this analysis.
        /// </summary>
        public List<Event> Events { get; set; }

        /// <summary>
        /// Gets or sets the project version associated with the analysis.
        /// </summary>
        public string ProjectVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this analysis was manually set as new code period baseline.
        /// </summary>
        public bool ManualNewCodePeriodBaseline { get; set; }

        /// <summary>
        /// Gets or sets the revision identifier (e.g., Git commit SHA) associated with this analysis.
        /// </summary>
        public string Revision { get; set; }

        /// <summary>
        /// Gets or sets the continuous integration system detected during analysis.
        /// </summary>
        public string DetectedCI { get; set; }
    }
}
