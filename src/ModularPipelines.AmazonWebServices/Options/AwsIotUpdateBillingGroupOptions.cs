using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-billing-group")]
public record AwsIotUpdateBillingGroupOptions(
[property: CommandSwitch("--billing-group-name")] string BillingGroupName,
[property: CommandSwitch("--billing-group-properties")] string BillingGroupProperties
) : AwsOptions
{
    [CommandSwitch("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}