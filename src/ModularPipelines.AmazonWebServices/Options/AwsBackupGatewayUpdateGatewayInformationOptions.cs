using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "update-gateway-information")]
public record AwsBackupGatewayUpdateGatewayInformationOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CommandSwitch("--gateway-display-name")]
    public string? GatewayDisplayName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}