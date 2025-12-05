using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "online-deployment", "create")]
public record AzMlOnlineDeploymentCreateOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--all-traffic")]
    public bool? AllTraffic { get; set; }

    [CliOption("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CliFlag("--local")]
    public bool? Local { get; set; }

    [CliFlag("--local-enable-gpu")]
    public bool? LocalEnableGpu { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--package-model")]
    public bool? PackageModel { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--skip-script-validation")]
    public bool? SkipScriptValidation { get; set; }

    [CliFlag("--vscode-debug")]
    public bool? VscodeDebug { get; set; }

    [CliFlag("--web")]
    public bool? Web { get; set; }
}