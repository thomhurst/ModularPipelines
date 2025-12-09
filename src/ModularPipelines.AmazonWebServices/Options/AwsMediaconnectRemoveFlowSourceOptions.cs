using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "remove-flow-source")]
public record AwsMediaconnectRemoveFlowSourceOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--source-arn")] string SourceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}