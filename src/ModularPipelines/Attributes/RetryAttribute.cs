namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies retry behavior for module execution.
/// If the module fails, it will be retried according to the specified policy.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class RetryAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the RetryAttribute class.
    /// </summary>
    /// <param name="count">The number of times to retry on failure.</param>
    public RetryAttribute(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Retry count must be non-negative.");
        }

        Count = count;
        BackoffSeconds = 1; // Default 1 second backoff
    }

    /// <summary>
    /// Gets the number of retry attempts.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Gets or sets the backoff duration in seconds between retries.
    /// Default is 1 second. Uses exponential backoff: delay = BackoffSeconds * attempt^2
    /// </summary>
    public int BackoffSeconds { get; set; }
}
