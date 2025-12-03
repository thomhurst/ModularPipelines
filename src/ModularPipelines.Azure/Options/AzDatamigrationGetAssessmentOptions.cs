using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datamigration", "get-assessment")]
public record AzDatamigrationGetAssessmentOptions : AzOptions
{
    [CliOption("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--output-folder")]
    public string? OutputFolder { get; set; }

    [CliFlag("--overwrite")]
    public bool? Overwrite { get; set; }
}