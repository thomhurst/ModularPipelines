namespace ModularPipelines.Distributed;

/// <summary>
/// Selects the role performed by a distributed pipeline instance.
/// </summary>
public enum DistributedRole
{
    /// <summary>
    /// Selects <see cref="Master"/> when the instance index is zero; otherwise selects <see cref="Worker"/>.
    /// </summary>
    Auto,

    /// <summary>
    /// Coordinates and executes distributed work.
    /// </summary>
    Master,

    /// <summary>
    /// Executes work assigned by the master.
    /// </summary>
    Worker
}
