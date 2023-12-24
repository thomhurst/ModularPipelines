using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appflow", "update-connector-profile")]
public record AwsAppflowUpdateConnectorProfileOptions(
[property: CommandSwitch("--connector-profile-name")] string ConnectorProfileName,
[property: CommandSwitch("--connection-mode")] string ConnectionMode,
[property: CommandSwitch("--connector-profile-config")] string ConnectorProfileConfig
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}