using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appmesh", "describe-virtual-service")]
public record AwsAppmeshDescribeVirtualServiceOptions(
[property: CommandSwitch("--mesh-name")] string MeshName,
[property: CommandSwitch("--virtual-service-name")] string VirtualServiceName
) : AwsOptions
{
    [CommandSwitch("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}