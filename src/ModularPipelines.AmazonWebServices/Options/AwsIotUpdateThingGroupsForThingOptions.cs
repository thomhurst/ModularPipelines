using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-thing-groups-for-thing")]
public record AwsIotUpdateThingGroupsForThingOptions : AwsOptions
{
    [CliOption("--thing-name")]
    public string? ThingName { get; set; }

    [CliOption("--thing-groups-to-add")]
    public string[]? ThingGroupsToAdd { get; set; }

    [CliOption("--thing-groups-to-remove")]
    public string[]? ThingGroupsToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}