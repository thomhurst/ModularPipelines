using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "get-layer-version-policy")]
public record AwsLambdaGetLayerVersionPolicyOptions(
[property: CommandSwitch("--layer-name")] string LayerName,
[property: CommandSwitch("--version-number")] long VersionNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}