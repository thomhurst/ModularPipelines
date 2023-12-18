using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "get-assessment")]
public record AzDatamigrationGetAssessmentOptions(
[property: CommandSwitch("--auth-key")] string AuthKey
) : AzOptions
{
    [CommandSwitch("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--output-folder")]
    public string? OutputFolder { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }
}