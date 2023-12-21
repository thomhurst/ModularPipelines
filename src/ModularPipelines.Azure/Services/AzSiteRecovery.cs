using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzSiteRecovery
{
    public AzSiteRecovery(
        AzSiteRecoveryAlertSetting alertSetting,
        AzSiteRecoveryEvent @event,
        AzSiteRecoveryFabric fabric,
        AzSiteRecoveryJob job,
        AzSiteRecoveryLogicalNetwork logicalNetwork,
        AzSiteRecoveryNetwork network,
        AzSiteRecoveryPolicy policy,
        AzSiteRecoveryProtectableItem protectableItem,
        AzSiteRecoveryProtectedItem protectedItem,
        AzSiteRecoveryProtectionContainer protectionContainer,
        AzSiteRecoveryRecoveryPlan recoveryPlan,
        AzSiteRecoveryRecoveryServicesProvider recoveryServicesProvider,
        AzSiteRecoveryReplicationEligibility replicationEligibility,
        AzSiteRecoveryVault vault,
        AzSiteRecoveryVmwareSite vmwareSite
    )
    {
        AlertSetting = alertSetting;
        Event = @event;
        Fabric = fabric;
        Job = job;
        LogicalNetwork = logicalNetwork;
        Network = network;
        Policy = policy;
        ProtectableItem = protectableItem;
        ProtectedItem = protectedItem;
        ProtectionContainer = protectionContainer;
        RecoveryPlan = recoveryPlan;
        RecoveryServicesProvider = recoveryServicesProvider;
        ReplicationEligibility = replicationEligibility;
        Vault = vault;
        VmwareSite = vmwareSite;
    }

    public AzSiteRecoveryAlertSetting AlertSetting { get; }

    public AzSiteRecoveryEvent Event { get; }

    public AzSiteRecoveryFabric Fabric { get; }

    public AzSiteRecoveryJob Job { get; }

    public AzSiteRecoveryLogicalNetwork LogicalNetwork { get; }

    public AzSiteRecoveryNetwork Network { get; }

    public AzSiteRecoveryPolicy Policy { get; }

    public AzSiteRecoveryProtectableItem ProtectableItem { get; }

    public AzSiteRecoveryProtectedItem ProtectedItem { get; }

    public AzSiteRecoveryProtectionContainer ProtectionContainer { get; }

    public AzSiteRecoveryRecoveryPlan RecoveryPlan { get; }

    public AzSiteRecoveryRecoveryServicesProvider RecoveryServicesProvider { get; }

    public AzSiteRecoveryReplicationEligibility ReplicationEligibility { get; }

    public AzSiteRecoveryVault Vault { get; }

    public AzSiteRecoveryVmwareSite VmwareSite { get; }
}