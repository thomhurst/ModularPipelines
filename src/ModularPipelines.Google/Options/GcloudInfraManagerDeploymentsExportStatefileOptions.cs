using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "deployments", "export-statefile")]
public record GcloudInfraManagerDeploymentsExportStatefileOptions(
[property: CliArgument] string Deployment,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--draft")]
    public bool? Draft { get; set; }
}