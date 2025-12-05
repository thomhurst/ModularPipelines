using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "start-resource-scan")]
public record AwsAccessanalyzerStartResourceScanOptions(
[property: CliOption("--analyzer-arn")] string AnalyzerArn,
[property: CliOption("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CliOption("--resource-owner-account")]
    public string? ResourceOwnerAccount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}