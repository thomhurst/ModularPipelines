using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "disassociate-app-block-builder-app-block")]
public record AwsAppstreamDisassociateAppBlockBuilderAppBlockOptions(
[property: CliOption("--app-block-arn")] string AppBlockArn,
[property: CliOption("--app-block-builder-name")] string AppBlockBuilderName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}