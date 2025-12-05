using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "describe")]
public record GcloudDatabaseMigrationConversionWorkspacesDescribeOptions(
[property: CliArgument] string ConversionWorkspace,
[property: CliArgument] string Region
) : GcloudOptions;