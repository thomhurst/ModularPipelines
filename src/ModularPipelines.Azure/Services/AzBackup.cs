using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzBackup
{
    public AzBackup(
        AzBackupContainer container,
        AzBackupItem item,
        AzBackupJob job,
        AzBackupPolicy policy,
        AzBackupProtectableItem protectableItem,
        AzBackupProtection protection,
        AzBackupRecoveryconfig recoveryconfig,
        AzBackupRecoverypoint recoverypoint,
        AzBackupRestore restore,
        AzBackupVault vault
    )
    {
        Container = container;
        Item = item;
        Job = job;
        Policy = policy;
        ProtectableItem = protectableItem;
        Protection = protection;
        Recoveryconfig = recoveryconfig;
        Recoverypoint = recoverypoint;
        Restore = restore;
        Vault = vault;
    }

    public AzBackupContainer Container { get; }

    public AzBackupItem Item { get; }

    public AzBackupJob Job { get; }

    public AzBackupPolicy Policy { get; }

    public AzBackupProtectableItem ProtectableItem { get; }

    public AzBackupProtection Protection { get; }

    public AzBackupRecoveryconfig Recoveryconfig { get; }

    public AzBackupRecoverypoint Recoverypoint { get; }

    public AzBackupRestore Restore { get; }

    public AzBackupVault Vault { get; }
}