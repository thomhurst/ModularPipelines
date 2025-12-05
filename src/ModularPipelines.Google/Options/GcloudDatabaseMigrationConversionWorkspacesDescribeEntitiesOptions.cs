using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "describe-entities")]
public record GcloudDatabaseMigrationConversionWorkspacesDescribeEntitiesOptions(
[property: CliArgument] string ConversionWorkspace,
[property: CliArgument] string Region,
[property: CliOption("--tree-type")] string TreeType
) : GcloudOptions
{
    [CliOption("--commit-id")]
    public string? CommitId { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliFlag("--uncommitted")]
    public bool? Uncommitted { get; set; }
}