using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "remove-flow-output")]
public record AwsMediaconnectRemoveFlowOutputOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--output-arn")] string OutputArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}