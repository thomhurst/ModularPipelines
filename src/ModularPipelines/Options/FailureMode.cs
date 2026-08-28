namespace ModularPipelines.Options;

/// <summary>
/// Defines how the pipeline should behave when module execution encounters exceptions.
/// </summary>
public enum FailureMode
{
    /// <summary>
    /// Stop pipeline execution immediately when the first exception occurs.
    /// </summary>
    FailFast,

    /// <summary>
    /// Continue running independent modules before evaluating failures.
    /// </summary>
    ContinueOnFailure,
}
