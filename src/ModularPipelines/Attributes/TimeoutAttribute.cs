namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies a timeout for module execution.
/// If the module exceeds this timeout, it will be cancelled and marked as timed out.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class TimeoutAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the TimeoutAttribute class with minutes.
    /// </summary>
    /// <param name="minutes">The timeout duration in minutes.</param>
    public TimeoutAttribute(int minutes)
    {
        Timeout = TimeSpan.FromMinutes(minutes);
    }

    /// <summary>
    /// Gets or sets the timeout in minutes.
    /// </summary>
    public int Minutes
    {
        get => (int)Timeout.TotalMinutes;
        set => Timeout = TimeSpan.FromMinutes(value);
    }

    /// <summary>
    /// Gets or sets the timeout in seconds.
    /// </summary>
    public int Seconds
    {
        get => (int)Timeout.TotalSeconds;
        set => Timeout = TimeSpan.FromSeconds(value);
    }

    /// <summary>
    /// Gets or sets the timeout in hours.
    /// </summary>
    public int Hours
    {
        get => (int)Timeout.TotalHours;
        set => Timeout = TimeSpan.FromHours(value);
    }

    /// <summary>
    /// Gets the configured timeout.
    /// </summary>
    public TimeSpan Timeout { get; private set; }
}
