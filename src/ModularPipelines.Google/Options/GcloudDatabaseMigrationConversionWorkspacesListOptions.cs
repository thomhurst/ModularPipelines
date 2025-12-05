using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "list")]
public record GcloudDatabaseMigrationConversionWorkspacesListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions;