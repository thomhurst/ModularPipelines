using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "deployments", "unlock")]
public record GcloudInfraManagerDeploymentsUnlockOptions(
[property: PositionalArgument] string Deployment,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--lock-id")] string LockId
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}