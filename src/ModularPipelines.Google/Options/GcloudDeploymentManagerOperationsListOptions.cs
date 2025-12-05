using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "operations", "list")]
public record GcloudDeploymentManagerOperationsListOptions : GcloudOptions
{
    [CliFlag("--simple-list")]
    public bool? SimpleList { get; set; }
}