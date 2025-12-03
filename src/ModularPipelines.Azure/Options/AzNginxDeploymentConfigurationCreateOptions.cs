using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nginx", "deployment", "configuration", "create")]
public record AzNginxDeploymentConfigurationCreateOptions(
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--deployment-name")] string DeploymentName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--files")]
    public string? Files { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--package")]
    public string? Package { get; set; }

    [CliOption("--protected-files")]
    public string? ProtectedFiles { get; set; }

    [CliOption("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CliOption("--root-file")]
    public string? RootFile { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}