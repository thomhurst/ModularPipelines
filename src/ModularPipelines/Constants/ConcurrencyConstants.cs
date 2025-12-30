namespace ModularPipelines.Constants;

/// <summary>
/// Constants used for concurrency and parallelism configuration.
/// </summary>
internal static class ConcurrencyConstants
{
    /// <summary>
    /// The multiplier applied to ProcessorCount to calculate default maximum parallelism.
    /// </summary>
    public const int ParallelismMultiplier = 4;
}
