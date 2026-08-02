namespace ModularPipelines.Git;

/// <summary>
/// Detects repository paths changed relative to a base revision.
/// </summary>
public interface IGitChanges
{
    /// <summary>
    /// Determines whether any changed path matches one of the supplied glob patterns.
    /// The merge base and changed path set, including staged and unstaged tracked changes, are computed
    /// once for each base revision per pipeline run. If the base revision is unavailable, the check
    /// conservatively returns <see langword="true"/>.
    /// </summary>
    /// <param name="pathPatterns">Repository-relative glob patterns to match.</param>
    /// <param name="baseReference">The revision whose merge base with HEAD starts the comparison.</param>
    /// <param name="cancellationToken">A token used to cancel Git operations.</param>
    /// <returns><see langword="true"/> when at least one changed path matches.</returns>
    Task<bool> HasChangesAsync(
        IEnumerable<string> pathPatterns,
        string baseReference = "origin/main",
        CancellationToken cancellationToken = default);
}
