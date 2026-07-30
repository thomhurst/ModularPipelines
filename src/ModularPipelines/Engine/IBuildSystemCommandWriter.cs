namespace ModularPipelines.Engine;

/// <summary>
/// Writes CI workflow commands directly to the original standard output stream.
/// </summary>
internal interface IBuildSystemCommandWriter
{
    /// <summary>
    /// Writes one complete build-system command as a single physical line.
    /// </summary>
    /// <param name="command">The formatted build-system command.</param>
    void WriteLine(string command);
}
