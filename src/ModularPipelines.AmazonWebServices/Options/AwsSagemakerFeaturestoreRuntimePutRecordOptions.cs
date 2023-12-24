using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-featurestore-runtime", "put-record")]
public record AwsSagemakerFeaturestoreRuntimePutRecordOptions(
[property: CommandSwitch("--feature-group-name")] string FeatureGroupName,
[property: CommandSwitch("--record")] string[] Record
) : AwsOptions
{
    [CommandSwitch("--target-stores")]
    public string[]? TargetStores { get; set; }

    [CommandSwitch("--ttl-duration")]
    public string? TtlDuration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}