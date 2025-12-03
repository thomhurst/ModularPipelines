using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "disable-import-findings-for-product")]
public record AwsSecurityhubDisableImportFindingsForProductOptions(
[property: CliOption("--product-subscription-arn")] string ProductSubscriptionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}