using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "import-rules")]
public record GcloudDatabaseMigrationConversionWorkspacesImportRulesOptions(
[property: PositionalArgument] string ConversionWorkspace,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--config-files")] string[] ConfigFiles,
[property: CommandSwitch("--file-format")] string FileFormat
) : GcloudOptions
{
    [BooleanCommandSwitch("--auto-commit")]
    public bool? AutoCommit { get; set; }
}