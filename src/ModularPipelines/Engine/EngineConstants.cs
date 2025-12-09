namespace ModularPipelines.Engine;

/// <summary>
/// Constants used throughout the module execution engine.
/// </summary>
internal static class EngineConstants
{
    /// <summary>
    /// Scheduler configuration constants.
    /// </summary>
    public static class Scheduler
    {
        /// <summary>
        /// Timeout for waiting on scheduler notifications before re-checking state.
        /// This periodic re-check ensures we don't miss signals due to race conditions.
        /// Value chosen as a balance between responsiveness and CPU usage.
        /// </summary>
        public static readonly TimeSpan NotificationWaitTimeout = TimeSpan.FromMilliseconds(100);
    }
}
