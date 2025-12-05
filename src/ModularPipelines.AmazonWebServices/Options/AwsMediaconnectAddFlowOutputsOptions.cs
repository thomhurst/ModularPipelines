using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "add-flow-outputs")]
public record AwsMediaconnectAddFlowOutputsOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--outputs")] string[] Outputs
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}