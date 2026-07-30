namespace ModularPipelines.Engine;

/// <summary>
/// Writes CI workflow commands without passing through Spectre rendering or console interception.
/// </summary>
internal sealed class BuildSystemCommandWriter : IBuildSystemCommandWriter
{
    private readonly TextWriter _originalStandardOutput;
    private readonly Lock _writeLock = new();

    internal BuildSystemCommandWriter(TextWriter originalStandardOutput)
    {
        ArgumentNullException.ThrowIfNull(originalStandardOutput);
        _originalStandardOutput = originalStandardOutput;
    }

    /// <inheritdoc />
    public void WriteLine(string command)
    {
        ArgumentException.ThrowIfNullOrEmpty(command);
        if (command.Contains('\n'))
        {
            throw new ArgumentException(
                "Build-system commands must contain exactly one physical line.",
                nameof(command));
        }

        lock (_writeLock)
        {
            _originalStandardOutput.WriteLine(command);
        }
    }
}
