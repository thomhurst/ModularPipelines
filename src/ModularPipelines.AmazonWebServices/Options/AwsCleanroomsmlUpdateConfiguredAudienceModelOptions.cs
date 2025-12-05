using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "update-configured-audience-model")]
public record AwsCleanroomsmlUpdateConfiguredAudienceModelOptions(
[property: CliOption("--configured-audience-model-arn")] string ConfiguredAudienceModelArn
) : AwsOptions
{
    [CliOption("--audience-model-arn")]
    public string? AudienceModelArn { get; set; }

    [CliOption("--audience-size-config")]
    public string? AudienceSizeConfig { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--min-matching-seed-size")]
    public int? MinMatchingSeedSize { get; set; }

    [CliOption("--output-config")]
    public string? OutputConfig { get; set; }

    [CliOption("--shared-audience-metrics")]
    public string[]? SharedAudienceMetrics { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}