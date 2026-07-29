namespace ModularPipelines.Git;

/// <summary>
/// Provides Git commands grouped by workflow.
/// </summary>
public interface IGitCommands
{
    IGitRepositoryCommands Repository { get; }

    IGitWorkingTreeCommands WorkingTree { get; }

    IGitBranchCommands Branches { get; }

    IGitRemoteCommands Remotes { get; }

    IGitHistoryCommands History { get; }

    IGitMaintenanceCommands Maintenance { get; }
}
