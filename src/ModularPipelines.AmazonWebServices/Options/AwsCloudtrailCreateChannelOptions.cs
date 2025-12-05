using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "create-channel")]
public record AwsCloudtrailCreateChannelOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--source")] string Source,
[property: CliOption("--destinations")] string[] Destinations
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}