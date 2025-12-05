using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("robomaker", "create-world-generation-job")]
public record AwsRobomakerCreateWorldGenerationJobOptions(
[property: CliOption("--template")] string Template,
[property: CliOption("--world-count")] string WorldCount
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--world-tags")]
    public IEnumerable<KeyValue>? WorldTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}