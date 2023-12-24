using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "update-configured-audience-model")]
public record AwsCleanroomsmlUpdateConfiguredAudienceModelOptions(
[property: CommandSwitch("--configured-audience-model-arn")] string ConfiguredAudienceModelArn
) : AwsOptions
{
    [CommandSwitch("--audience-model-arn")]
    public string? AudienceModelArn { get; set; }

    [CommandSwitch("--audience-size-config")]
    public string? AudienceSizeConfig { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--min-matching-seed-size")]
    public int? MinMatchingSeedSize { get; set; }

    [CommandSwitch("--output-config")]
    public string? OutputConfig { get; set; }

    [CommandSwitch("--shared-audience-metrics")]
    public string[]? SharedAudienceMetrics { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}