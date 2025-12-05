using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "list-background-jobs")]
public record GcloudDatabaseMigrationConversionWorkspacesListBackgroundJobsOptions(
[property: CliArgument] string ConversionWorkspace,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--max-size")]
    public string? MaxSize { get; set; }

    [CliFlag("--most-recent-per-job-type")]
    public bool? MostRecentPerJobType { get; set; }
}