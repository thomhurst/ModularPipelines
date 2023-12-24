using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguru-security", "get-scan")]
public record AwsCodeguruSecurityGetScanOptions(
[property: CommandSwitch("--scan-name")] string ScanName
) : AwsOptions
{
    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}