using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privatenetworks", "get-network-resource")]
public record AwsPrivatenetworksGetNetworkResourceOptions(
[property: CommandSwitch("--network-resource-arn")] string NetworkResourceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}