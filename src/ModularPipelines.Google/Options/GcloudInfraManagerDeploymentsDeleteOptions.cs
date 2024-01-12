using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "deployments", "delete")]
public record GcloudInfraManagerDeploymentsDeleteOptions(
[property: PositionalArgument] string Deployment,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--delete-policy")]
    public string? DeletePolicy { get; set; }
}