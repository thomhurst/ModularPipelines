using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "online-deployment", "update")]
public record AzMlOnlineDeploymentUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--local")]
    public bool? Local { get; set; }

    [CliFlag("--local-enable-gpu")]
    public bool? LocalEnableGpu { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--skip-script-validation")]
    public bool? SkipScriptValidation { get; set; }

    [CliFlag("--vscode-debug")]
    public bool? VscodeDebug { get; set; }

    [CliFlag("--web")]
    public bool? Web { get; set; }
}