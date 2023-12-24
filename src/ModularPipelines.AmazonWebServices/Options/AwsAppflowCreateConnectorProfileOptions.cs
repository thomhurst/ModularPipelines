using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appflow", "create-connector-profile")]
public record AwsAppflowCreateConnectorProfileOptions(
[property: CommandSwitch("--connector-profile-name")] string ConnectorProfileName,
[property: CommandSwitch("--connector-type")] string ConnectorType,
[property: CommandSwitch("--connection-mode")] string ConnectionMode,
[property: CommandSwitch("--connector-profile-config")] string ConnectorProfileConfig
) : AwsOptions
{
    [CommandSwitch("--kms-arn")]
    public string? KmsArn { get; set; }

    [CommandSwitch("--connector-label")]
    public string? ConnectorLabel { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}