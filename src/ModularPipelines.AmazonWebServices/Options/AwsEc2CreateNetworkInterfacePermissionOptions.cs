using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-network-interface-permission")]
public record AwsEc2CreateNetworkInterfacePermissionOptions(
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId,
[property: CommandSwitch("--permission")] string Permission
) : AwsOptions
{
    [CommandSwitch("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CommandSwitch("--aws-service")]
    public string? AwsService { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}