using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-resource-config")]
public record AwsConfigservicePutResourceConfigOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--schema-version-id")] string SchemaVersionId,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--configuration")] string Configuration
) : AwsOptions
{
    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}