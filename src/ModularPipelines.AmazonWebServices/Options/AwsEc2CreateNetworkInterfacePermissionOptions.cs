using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-network-interface-permission")]
public record AwsEc2CreateNetworkInterfacePermissionOptions(
[property: CliOption("--network-interface-id")] string NetworkInterfaceId,
[property: CliOption("--permission")] string Permission
) : AwsOptions
{
    [CliOption("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CliOption("--aws-service")]
    public string? AwsService { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}