using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "create-configured-audience-model")]
public record AwsCleanroomsmlCreateConfiguredAudienceModelOptions(
[property: CommandSwitch("--audience-model-arn")] string AudienceModelArn,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--output-config")] string OutputConfig,
[property: CommandSwitch("--shared-audience-metrics")] string[] SharedAudienceMetrics
) : AwsOptions
{
    [CommandSwitch("--audience-size-config")]
    public string? AudienceSizeConfig { get; set; }

    [CommandSwitch("--child-resource-tag-on-create-policy")]
    public string? ChildResourceTagOnCreatePolicy { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--min-matching-seed-size")]
    public int? MinMatchingSeedSize { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}