using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "describe-entities")]
public record GcloudDatabaseMigrationConversionWorkspacesDescribeEntitiesOptions(
[property: PositionalArgument] string ConversionWorkspace,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--tree-type")] string TreeType
) : GcloudOptions
{
    [CommandSwitch("--commit-id")]
    public string? CommitId { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("--uncommitted")]
    public bool? Uncommitted { get; set; }
}