using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "deployments", "unlock")]
public record GcloudInfraManagerDeploymentsUnlockOptions(
[property: CliArgument] string Deployment,
[property: CliArgument] string Location,
[property: CliOption("--lock-id")] string LockId
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}