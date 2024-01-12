using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "describe-ddls")]
public record GcloudDatabaseMigrationConversionWorkspacesDescribeDdlsOptions(
[property: PositionalArgument] string ConversionWorkspace,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--commit-id")]
    public string? CommitId { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--tree-type")]
    public string? TreeType { get; set; }

    [BooleanCommandSwitch("--uncommitted")]
    public bool? Uncommitted { get; set; }
}