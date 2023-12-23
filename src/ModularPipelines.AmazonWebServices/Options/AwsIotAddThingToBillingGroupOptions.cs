using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "add-thing-to-billing-group")]
public record AwsIotAddThingToBillingGroupOptions : AwsOptions
{
    [CommandSwitch("--billing-group-name")]
    public string? BillingGroupName { get; set; }

    [CommandSwitch("--billing-group-arn")]
    public string? BillingGroupArn { get; set; }

    [CommandSwitch("--thing-name")]
    public string? ThingName { get; set; }

    [CommandSwitch("--thing-arn")]
    public string? ThingArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}