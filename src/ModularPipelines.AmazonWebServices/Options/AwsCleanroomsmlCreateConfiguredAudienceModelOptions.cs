using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "create-configured-audience-model")]
public record AwsCleanroomsmlCreateConfiguredAudienceModelOptions(
[property: CliOption("--audience-model-arn")] string AudienceModelArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--output-config")] string OutputConfig,
[property: CliOption("--shared-audience-metrics")] string[] SharedAudienceMetrics
) : AwsOptions
{
    [CliOption("--audience-size-config")]
    public string? AudienceSizeConfig { get; set; }

    [CliOption("--child-resource-tag-on-create-policy")]
    public string? ChildResourceTagOnCreatePolicy { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--min-matching-seed-size")]
    public int? MinMatchingSeedSize { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}