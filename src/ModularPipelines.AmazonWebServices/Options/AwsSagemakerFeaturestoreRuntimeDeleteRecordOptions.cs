using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-featurestore-runtime", "delete-record")]
public record AwsSagemakerFeaturestoreRuntimeDeleteRecordOptions(
[property: CommandSwitch("--feature-group-name")] string FeatureGroupName,
[property: CommandSwitch("--record-identifier-value-as-string")] string RecordIdentifierValueAsString,
[property: CommandSwitch("--event-time")] string EventTime
) : AwsOptions
{
    [CommandSwitch("--target-stores")]
    public string[]? TargetStores { get; set; }

    [CommandSwitch("--deletion-mode")]
    public string? DeletionMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}