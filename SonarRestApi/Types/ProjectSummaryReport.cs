﻿namespace SonarRestService.Types
{
    /// <summary>
    /// Summary report for project
    /// </summary>
    public class ProjectSummaryReport
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>
        /// The resource.
        /// </value>
        public Resource Resource { get; set; }

        /// <summary>
        /// Gets or sets all lines
        /// </summary>
        public long Lines { get; set; }

        /// <summary>
        /// Gets or sets issues
        /// </summary>
        public long Issues { get; set; }

        /// <summary>
        /// Gets or sets new issues
        /// </summary>
        public long NewIssues { get; set; }

        /// <summary>
        /// Gets or sets new technical debt
        /// </summary>
        public long NewTechnicalDebt { get; set; }

        /// <summary>
        /// Gets or sets technical debt
        /// </summary>
        public long TechnicalDebt { get; set; }

        /// <summary>
        /// Gets or sets cognitive complexity
        /// </summary>
        public long CognitiveComplexity { get; set; }

        /// <summary>
        /// Gets or sets cyclomatic complexity
        /// </summary>
        public long CyclomaticComplexity { get; set; }

        /// <summary>
        /// Gets or sets the language distribuition
        /// </summary>
        public string LinesOfCodeLangDistribution { get; set; }

        /// <summary>
        /// Gets or sets the uncovered conditons.
        /// </summary>
        /// <value>
        /// The uncovered conditons.
        /// </value>
        public long LinesOfCode { get; set; }

        /// <summary>
        /// Gets or sets the uncovered lines.
        /// </summary>
        /// <value>
        /// The uncovered lines.
        /// </value>
        public long NewLines { get; set; }

        /// <summary>
        /// Gets or sets the new coverage.
        /// </summary>
        /// <value>
        /// The new coverage.
        /// </value>
        public decimal NewCoverage { get; set; }

        /// <summary>
        /// Gets or sets the coverage.
        /// </summary>
        /// <value>
        /// The coverage.
        /// </value>
        public decimal Coverage { get; set; }
    }
}
