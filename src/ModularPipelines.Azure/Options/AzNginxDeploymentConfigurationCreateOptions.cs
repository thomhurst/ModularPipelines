using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nginx", "deployment", "configuration", "create")]
public record AzNginxDeploymentConfigurationCreateOptions(
[property: CommandSwitch("--configuration-name")] string ConfigurationName,
[property: CommandSwitch("--deployment-name")] string DeploymentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--files")]
    public string? Files { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--package")]
    public string? Package { get; set; }

    [CommandSwitch("--protected-files")]
    public string? ProtectedFiles { get; set; }

    [CommandSwitch("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CommandSwitch("--root-file")]
    public string? RootFile { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}