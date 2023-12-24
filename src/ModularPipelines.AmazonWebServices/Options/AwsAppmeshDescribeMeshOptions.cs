using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appmesh", "describe-mesh")]
public record AwsAppmeshDescribeMeshOptions(
[property: CommandSwitch("--mesh-name")] string MeshName
) : AwsOptions
{
    [CommandSwitch("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}