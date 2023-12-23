using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-featurestore-runtime", "batch-get-record")]
public record AwsSagemakerFeaturestoreRuntimeBatchGetRecordOptions(
[property: CommandSwitch("--identifiers")] string[] Identifiers
) : AwsOptions
{
    [CommandSwitch("--expiration-time-response")]
    public string? ExpirationTimeResponse { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}