using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "start-image-builder")]
public record AwsAppstreamStartImageBuilderOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--appstream-agent-version")]
    public string? AppstreamAgentVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}