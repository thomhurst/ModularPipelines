using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "create-gateway")]
public record AwsBackupGatewayCreateGatewayOptions(
[property: CommandSwitch("--activation-key")] string ActivationKey,
[property: CommandSwitch("--gateway-display-name")] string GatewayDisplayName,
[property: CommandSwitch("--gateway-type")] string GatewayType
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}