using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "workspace-hub", "delete")]
public record AzMlWorkspaceHubDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--all-resources")]
    public bool? AllResources { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--permanently-delete")]
    public bool? PermanentlyDelete { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}