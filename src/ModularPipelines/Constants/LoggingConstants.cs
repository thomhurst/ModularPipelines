namespace ModularPipelines.Constants;

/// <summary>
/// Constants used for logging and output formatting.
/// </summary>
internal static class LoggingConstants
{
    /// <summary>
    /// The string used to mask secret values in logs.
    /// </summary>
    public const string SecretMask = "**********";

    /// <summary>
    /// The string used to mask command arguments when they should not be displayed.
    /// </summary>
    public const string CommandMask = "********";

    /// <summary>
    /// Default maximum body size (4KB) for HTTP logging.
    /// </summary>
    public const int DefaultMaxBodySizeToLog = 4096;

    /// <summary>
    /// Full logging maximum body size (64KB) for HTTP logging.
    /// </summary>
    public const int FullLoggingMaxBodySize = 65536;
}
