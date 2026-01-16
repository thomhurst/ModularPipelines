namespace ModularPipelines.Console;

/// <summary>
/// Controls pausing and resuming the progress display for output flushing.
/// </summary>
internal interface IProgressController
{
    /// <summary>
    /// Gets whether this controller supports dynamic display (interactive terminal).
    /// </summary>
    bool IsInteractive { get; }

    /// <summary>
    /// Pauses the progress display to allow console output.
    /// On interactive terminals, this clears the progress display.
    /// On CI, this is a no-op.
    /// </summary>
    Task PauseAsync();

    /// <summary>
    /// Resumes the progress display after output.
    /// On interactive terminals, this redraws the progress state.
    /// On CI, this is a no-op.
    /// </summary>
    Task ResumeAsync();
}
