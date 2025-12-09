using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "remove-thing-from-thing-group")]
public record AwsIotRemoveThingFromThingGroupOptions : AwsOptions
{
    [CliOption("--thing-group-name")]
    public string? ThingGroupName { get; set; }

    [CliOption("--thing-group-arn")]
    public string? ThingGroupArn { get; set; }

    [CliOption("--thing-name")]
    public string? ThingName { get; set; }

    [CliOption("--thing-arn")]
    public string? ThingArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}