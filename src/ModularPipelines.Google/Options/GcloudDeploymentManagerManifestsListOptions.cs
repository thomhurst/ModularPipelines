using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "manifests", "list")]
public record GcloudDeploymentManagerManifestsListOptions(
[property: CliOption("--deployment")] string Deployment
) : GcloudOptions
{
    [CliFlag("--simple-list")]
    public bool? SimpleList { get; set; }
}