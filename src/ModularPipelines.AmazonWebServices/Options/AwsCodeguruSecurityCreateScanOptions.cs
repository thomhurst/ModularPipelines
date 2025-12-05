using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-security", "create-scan")]
public record AwsCodeguruSecurityCreateScanOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--scan-name")] string ScanName
) : AwsOptions
{
    [CliOption("--analysis-type")]
    public string? AnalysisType { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--scan-type")]
    public string? ScanType { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}