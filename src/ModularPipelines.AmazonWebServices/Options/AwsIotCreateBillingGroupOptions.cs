using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-billing-group")]
public record AwsIotCreateBillingGroupOptions(
[property: CommandSwitch("--billing-group-name")] string BillingGroupName
) : AwsOptions
{
    [CommandSwitch("--billing-group-properties")]
    public string? BillingGroupProperties { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}