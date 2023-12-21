using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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