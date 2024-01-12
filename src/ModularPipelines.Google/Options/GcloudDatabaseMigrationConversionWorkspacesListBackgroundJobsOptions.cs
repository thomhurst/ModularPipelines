using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "list-background-jobs")]
public record GcloudDatabaseMigrationConversionWorkspacesListBackgroundJobsOptions(
[property: PositionalArgument] string ConversionWorkspace,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--max-size")]
    public string? MaxSize { get; set; }

    [BooleanCommandSwitch("--most-recent-per-job-type")]
    public bool? MostRecentPerJobType { get; set; }
}