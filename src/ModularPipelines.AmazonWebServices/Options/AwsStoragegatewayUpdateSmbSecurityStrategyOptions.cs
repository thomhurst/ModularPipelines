using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "update-smb-security-strategy")]
public record AwsStoragegatewayUpdateSmbSecurityStrategyOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--smb-security-strategy")] string SmbSecurityStrategy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}