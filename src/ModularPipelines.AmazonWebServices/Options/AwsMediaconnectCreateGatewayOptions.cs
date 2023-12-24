using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "create-gateway")]
public record AwsMediaconnectCreateGatewayOptions(
[property: CommandSwitch("--egress-cidr-blocks")] string[] EgressCidrBlocks,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--networks")] string[] Networks
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}