using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "batch-disable-standards")]
public record AwsSecurityhubBatchDisableStandardsOptions(
[property: CommandSwitch("--standards-subscription-arns")] string[] StandardsSubscriptionArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}