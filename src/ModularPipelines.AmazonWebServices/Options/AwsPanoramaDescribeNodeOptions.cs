using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("panorama", "describe-node")]
public record AwsPanoramaDescribeNodeOptions(
[property: CommandSwitch("--node-id")] string NodeId
) : AwsOptions
{
    [CommandSwitch("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}