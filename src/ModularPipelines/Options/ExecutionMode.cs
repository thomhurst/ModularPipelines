namespace ModularPipelines.Options;

/// <summary>
/// Defines how the pipeline should behave when module execution encounters exceptions.
/// </summary>
public enum ExecutionMode
{
    /// <summary>
    /// Stop pipeline execution immediately when the first exception occurs.
    /// </summary>
    StopOnFirstException,
    
    /// <summary>
    /// Continue running all modules and wait for all to complete before evaluating failures.
    /// </summary>
    WaitForAllModules,
}