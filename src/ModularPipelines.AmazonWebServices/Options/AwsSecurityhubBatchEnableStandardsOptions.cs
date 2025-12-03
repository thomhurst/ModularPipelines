using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "batch-enable-standards")]
public record AwsSecurityhubBatchEnableStandardsOptions(
[property: CliOption("--standards-subscription-requests")] string[] StandardsSubscriptionRequests
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}