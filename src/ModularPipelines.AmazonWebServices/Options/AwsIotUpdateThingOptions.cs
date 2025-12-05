using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-thing")]
public record AwsIotUpdateThingOptions(
[property: CliOption("--thing-name")] string ThingName
) : AwsOptions
{
    [CliOption("--thing-type-name")]
    public string? ThingTypeName { get; set; }

    [CliOption("--attribute-payload")]
    public string? AttributePayload { get; set; }

    [CliOption("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}