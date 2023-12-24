using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector-scan", "scan-sbom")]
public record AwsInspectorScanScanSbomOptions(
[property: CommandSwitch("--sbom")] string Sbom
) : AwsOptions
{
    [CommandSwitch("--output-format")]
    public string? OutputFormat { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}