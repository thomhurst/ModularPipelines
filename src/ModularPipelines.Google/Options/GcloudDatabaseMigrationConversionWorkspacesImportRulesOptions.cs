using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "import-rules")]
public record GcloudDatabaseMigrationConversionWorkspacesImportRulesOptions(
[property: CliArgument] string ConversionWorkspace,
[property: CliArgument] string Region,
[property: CliOption("--config-files")] string[] ConfigFiles,
[property: CliOption("--file-format")] string FileFormat
) : GcloudOptions
{
    [CliFlag("--auto-commit")]
    public bool? AutoCommit { get; set; }
}