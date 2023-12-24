using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "delete-layer-version")]
public record AwsLambdaDeleteLayerVersionOptions(
[property: CommandSwitch("--layer-name")] string LayerName,
[property: CommandSwitch("--version-number")] long VersionNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}