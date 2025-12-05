using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-thing-type")]
public record AwsIotCreateThingTypeOptions(
[property: CliOption("--thing-type-name")] string ThingTypeName
) : AwsOptions
{
    [CliOption("--thing-type-properties")]
    public string? ThingTypeProperties { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}