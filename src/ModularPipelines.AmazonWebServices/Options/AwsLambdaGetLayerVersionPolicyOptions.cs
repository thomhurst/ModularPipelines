using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "get-layer-version-policy")]
public record AwsLambdaGetLayerVersionPolicyOptions(
[property: CliOption("--layer-name")] string LayerName,
[property: CliOption("--version-number")] long VersionNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}