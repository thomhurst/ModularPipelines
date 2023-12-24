using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "delete-gateway")]
public record AwsBackupGatewayDeleteGatewayOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}