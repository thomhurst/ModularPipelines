using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "create-variant-store")]
public record AwsOmicsCreateVariantStoreOptions(
[property: CliOption("--reference")] string Reference
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--sse-config")]
    public string? SseConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}