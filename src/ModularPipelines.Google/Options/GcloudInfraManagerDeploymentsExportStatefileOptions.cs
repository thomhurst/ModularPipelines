using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "deployments", "export-statefile")]
public record GcloudInfraManagerDeploymentsExportStatefileOptions(
[property: PositionalArgument] string Deployment,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--draft")]
    public bool? Draft { get; set; }
}