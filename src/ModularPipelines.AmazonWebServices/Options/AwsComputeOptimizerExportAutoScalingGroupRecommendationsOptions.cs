using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute-optimizer", "export-auto-scaling-group-recommendations")]
public record AwsComputeOptimizerExportAutoScalingGroupRecommendationsOptions(
[property: CommandSwitch("--s3-destination-config")] string S3DestinationConfig
) : AwsOptions
{
    [CommandSwitch("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--fields-to-export")]
    public string[]? FieldsToExport { get; set; }

    [CommandSwitch("--file-format")]
    public string? FileFormat { get; set; }

    [CommandSwitch("--recommendation-preferences")]
    public string? RecommendationPreferences { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}