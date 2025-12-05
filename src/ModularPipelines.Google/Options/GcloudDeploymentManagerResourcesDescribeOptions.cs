using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "resources", "describe")]
public record GcloudDeploymentManagerResourcesDescribeOptions(
[property: CliArgument] string Resource
) : GcloudOptions
{
    [CliOption("--deployment")]
    public string? Deployment { get; set; }
}