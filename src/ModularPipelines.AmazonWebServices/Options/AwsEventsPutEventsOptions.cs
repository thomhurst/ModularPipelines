using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "put-events")]
public record AwsEventsPutEventsOptions(
[property: CliOption("--entries")] string[] Entries
) : AwsOptions
{
    [CliOption("--endpoint-id")]
    public string? EndpointId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}