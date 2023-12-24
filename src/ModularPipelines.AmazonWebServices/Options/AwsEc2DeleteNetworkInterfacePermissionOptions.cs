using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-network-interface-permission")]
public record AwsEc2DeleteNetworkInterfacePermissionOptions(
[property: CommandSwitch("--network-interface-permission-id")] string NetworkInterfacePermissionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}