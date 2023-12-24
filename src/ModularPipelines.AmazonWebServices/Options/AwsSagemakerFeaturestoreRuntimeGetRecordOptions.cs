using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-featurestore-runtime", "get-record")]
public record AwsSagemakerFeaturestoreRuntimeGetRecordOptions(
[property: CommandSwitch("--feature-group-name")] string FeatureGroupName,
[property: CommandSwitch("--record-identifier-value-as-string")] string RecordIdentifierValueAsString
) : AwsOptions
{
    [CommandSwitch("--feature-names")]
    public string[]? FeatureNames { get; set; }

    [CommandSwitch("--expiration-time-response")]
    public string? ExpirationTimeResponse { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}