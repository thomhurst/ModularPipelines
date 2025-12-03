using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "batch-deployment", "create")]
public record AzMlBatchDeploymentCreateOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--set-default")]
    public bool? SetDefault { get; set; }

    [CliFlag("--skip-script-validation")]
    public bool? SkipScriptValidation { get; set; }
}