using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "remove-thing-from-billing-group")]
public record AwsIotRemoveThingFromBillingGroupOptions : AwsOptions
{
    [CliOption("--billing-group-name")]
    public string? BillingGroupName { get; set; }

    [CliOption("--billing-group-arn")]
    public string? BillingGroupArn { get; set; }

    [CliOption("--thing-name")]
    public string? ThingName { get; set; }

    [CliOption("--thing-arn")]
    public string? ThingArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}