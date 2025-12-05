using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworkscm", "export-server-engine-attribute")]
public record AwsOpsworkscmExportServerEngineAttributeOptions(
[property: CliOption("--export-attribute-name")] string ExportAttributeName,
[property: CliOption("--server-name")] string ServerName
) : AwsOptions
{
    [CliOption("--input-attributes")]
    public string[]? InputAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}