using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "add-flow-sources")]
public record AwsMediaconnectAddFlowSourcesOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--sources")] string[] Sources
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}