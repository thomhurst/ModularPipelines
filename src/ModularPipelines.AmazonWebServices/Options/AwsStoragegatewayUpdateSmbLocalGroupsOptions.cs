using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "update-smb-local-groups")]
public record AwsStoragegatewayUpdateSmbLocalGroupsOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--smb-local-groups")] string SmbLocalGroups
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}