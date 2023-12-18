using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery")]
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

