using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "deployments", "delete")]
public record GcloudInfraManagerDeploymentsDeleteOptions(
[property: CliArgument] string Deployment,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--delete-policy")]
    public string? DeletePolicy { get; set; }
}