using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "batch-disable-standards")]
public record AwsSecurityhubBatchDisableStandardsOptions(
[property: CliOption("--standards-subscription-arns")] string[] StandardsSubscriptionArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}