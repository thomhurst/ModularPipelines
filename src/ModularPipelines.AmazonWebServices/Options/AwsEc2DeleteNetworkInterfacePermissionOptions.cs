using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-network-interface-permission")]
public record AwsEc2DeleteNetworkInterfacePermissionOptions(
[property: CliOption("--network-interface-permission-id")] string NetworkInterfacePermissionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}