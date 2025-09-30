using ModularPipelines.Models;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Models;

/// <summary>
/// Represents the result of test execution, including both test results and coverage files.
/// </summary>
public sealed class TestExecutionResult
{
    /// <summary>
    /// Gets the command results from running the tests.
    /// </summary>
    public required CommandResult[] TestResults { get; init; }

    /// <summary>
    /// Gets the coverage files generated during test execution.
    /// These files are automatically transferred to the orchestrator in distributed execution mode.
    /// </summary>
    public required List<File> CoverageFiles { get; init; }
}
