using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-thing-group")]
public record AwsIotUpdateThingGroupOptions(
[property: CliOption("--thing-group-name")] string ThingGroupName,
[property: CliOption("--thing-group-properties")] string ThingGroupProperties
) : AwsOptions
{
    [CliOption("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}