using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-smb-local-groups")]
public record AwsStoragegatewayUpdateSmbLocalGroupsOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--smb-local-groups")] string SmbLocalGroups
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}