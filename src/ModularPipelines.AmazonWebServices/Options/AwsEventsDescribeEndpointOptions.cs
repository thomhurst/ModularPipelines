using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "describe-endpoint")]
public record AwsEventsDescribeEndpointOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--home-region")]
    public string? HomeRegion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}