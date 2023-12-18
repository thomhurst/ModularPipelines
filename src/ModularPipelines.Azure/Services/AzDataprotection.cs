using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDataprotection
{
    public AzDataprotection(
        AzDataprotectionBackupInstance backupInstance,
        AzDataprotectionBackupPolicy backupPolicy,
        AzDataprotectionBackupVault backupVault,
        AzDataprotectionJob job,
        AzDataprotectionRecoveryPoint recoveryPoint,
        AzDataprotectionResourceGuard resourceGuard,
        AzDataprotectionRestorableTimeRange restorableTimeRange
    )
    {
        BackupInstance = backupInstance;
        BackupPolicy = backupPolicy;
        BackupVault = backupVault;
        Job = job;
        RecoveryPoint = recoveryPoint;
        ResourceGuard = resourceGuard;
        RestorableTimeRange = restorableTimeRange;
    }

    public AzDataprotectionBackupInstance BackupInstance { get; }

    public AzDataprotectionBackupPolicy BackupPolicy { get; }

    public AzDataprotectionBackupVault BackupVault { get; }

    public AzDataprotectionJob Job { get; }

    public AzDataprotectionRecoveryPoint RecoveryPoint { get; }

    public AzDataprotectionResourceGuard ResourceGuard { get; }

    public AzDataprotectionRestorableTimeRange RestorableTimeRange { get; }
}