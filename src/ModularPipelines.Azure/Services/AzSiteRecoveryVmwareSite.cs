using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("site-recovery")]
public class AzSiteRecoveryVmwareSite
{
    public AzSiteRecoveryVmwareSite(
        AzSiteRecoveryVmwareSiteMachine machine,
        AzSiteRecoveryVmwareSiteRunAsAccount runAsAccount
    )
    {
        Machine = machine;
        RunAsAccount = runAsAccount;
    }

    public AzSiteRecoveryVmwareSiteMachine Machine { get; }

    public AzSiteRecoveryVmwareSiteRunAsAccount RunAsAccount { get; }
}