using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "resources", "list")]
public record GcloudDeploymentManagerResourcesListOptions : GcloudOptions
{
    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliFlag("--simple-list")]
    public bool? SimpleList { get; set; }
}