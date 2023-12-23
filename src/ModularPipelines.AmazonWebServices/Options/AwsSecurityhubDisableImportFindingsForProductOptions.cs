using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "disable-import-findings-for-product")]
public record AwsSecurityhubDisableImportFindingsForProductOptions(
[property: CommandSwitch("--product-subscription-arn")] string ProductSubscriptionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}