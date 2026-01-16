using ModularPipelines.Models;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when the pipeline completes with one or more failed modules.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown after the pipeline summary has been printed to indicate
/// that the pipeline did not complete successfully. This ensures that CI/CD systems
/// receive a non-zero exit code when the pipeline fails.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>After the pipeline summary is printed</item>
/// <item>When one or more modules have failed</item>
/// </list>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="Summary"/> - The pipeline summary containing details of all module results</item>
/// <item><see cref="FailedModules"/> - List of module names that failed</item>
/// </list>
/// </remarks>
/// <seealso cref="PipelineException"/>
public class PipelineFailedException : PipelineException
{
    /// <summary>
    /// Gets the pipeline summary containing details of all module results.
    /// </summary>
    public PipelineSummary Summary { get; }

    /// <summary>
    /// Gets the names of the modules that failed.
    /// </summary>
    public IReadOnlyList<string> FailedModules { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineFailedException"/> class.
    /// </summary>
    /// <param name="summary">The pipeline summary.</param>
    /// <param name="failedModules">The names of modules that failed.</param>
    public PipelineFailedException(PipelineSummary summary, IReadOnlyList<string> failedModules)
        : base(FormatMessage(failedModules))
    {
        Summary = summary;
        FailedModules = failedModules;
    }

    private static string FormatMessage(IReadOnlyList<string> failedModules)
    {
        if (failedModules.Count == 1)
        {
            return $"Pipeline failed: {failedModules[0]} failed.";
        }

        return $"Pipeline failed: {failedModules.Count} modules failed ({string.Join(", ", failedModules)}).";
    }
}
