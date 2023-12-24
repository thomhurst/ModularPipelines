using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "create-annotation-store")]
public record AwsOmicsCreateAnnotationStoreOptions(
[property: CommandSwitch("--store-format")] string StoreFormat
) : AwsOptions
{
    [CommandSwitch("--reference")]
    public string? Reference { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--version-name")]
    public string? VersionName { get; set; }

    [CommandSwitch("--sse-config")]
    public string? SseConfig { get; set; }

    [CommandSwitch("--store-options")]
    public string? StoreOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}