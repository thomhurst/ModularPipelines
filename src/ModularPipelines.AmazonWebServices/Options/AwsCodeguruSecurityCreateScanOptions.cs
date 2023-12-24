using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguru-security", "create-scan")]
public record AwsCodeguruSecurityCreateScanOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--scan-name")] string ScanName
) : AwsOptions
{
    [CommandSwitch("--analysis-type")]
    public string? AnalysisType { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--scan-type")]
    public string? ScanType { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}