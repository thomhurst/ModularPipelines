using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "create-annotation-store")]
public record AwsOmicsCreateAnnotationStoreOptions(
[property: CliOption("--store-format")] string StoreFormat
) : AwsOptions
{
    [CliOption("--reference")]
    public string? Reference { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--version-name")]
    public string? VersionName { get; set; }

    [CliOption("--sse-config")]
    public string? SseConfig { get; set; }

    [CliOption("--store-options")]
    public string? StoreOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}