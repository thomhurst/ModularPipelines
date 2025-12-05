using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "delete-dynamic-thing-group")]
public record AwsIotDeleteDynamicThingGroupOptions(
[property: CliOption("--thing-group-name")] string ThingGroupName
) : AwsOptions
{
    [CliOption("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}