using ModularPipelines.Engine;
using ModularPipelines.Engine.BuildSystemFormatters;
using ModularPipelines.FileSystem;
using ModularPipelines.Secrets;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Detects the build system and publishes masked CI metadata through its native commands and files.
/// </summary>
internal class BuildSystemContext(
    IBuildSystemDetector detector,
    IEnvironmentVariablesContext variables,
    IBuildSystemCommandWriter commandWriter,
    IBuildSystemFormatterProvider formatters,
    ISecretObfuscator obfuscator,
    BuildSystemFileWriter fileWriter,
    IFileSystemProvider files) : IBuildSystemContext
{
    /// <inheritdoc />
    public BuildSystem Current => detector.Current;

    /// <inheritdoc />
    public bool IsCI => detector.IsCI;

    /// <inheritdoc />
    public bool Is(BuildSystem buildSystem) => detector.Is(buildSystem);

    /// <inheritdoc />
    public Task SetOutputAsync(string name, string value, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(value);
        if (name.Any(c => !char.IsAsciiLetterOrDigit(c) && c is not '_' and not '-' and not '.') ||
            obfuscator.Obfuscate(name, null) != name)
        {
            throw new ArgumentException("Output names must be identifiers and cannot contain registered secrets.", nameof(name));
        }

        cancellationToken.ThrowIfCancellationRequested();
        if (!IsCI)
        {
            return Task.CompletedTask;
        }

        if (Current == BuildSystem.GitHubActions)
        {
            return fileWriter.AppendOutputAsync(GetEnvironmentFile("GITHUB_OUTPUT"), name, value, cancellationToken);
        }

        WriteCommand(formatters.GetFormatter(Current).GetOutputCommand(name, Mask(value)), cancellationToken);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task AppendSummaryAsync(string markdown, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(markdown);
        cancellationToken.ThrowIfCancellationRequested();
        if (!IsCI)
        {
            return;
        }

        if (Current == BuildSystem.GitHubActions)
        {
            if (!await fileWriter.AppendGitHubSummaryAsync(GetEnvironmentFile("GITHUB_STEP_SUMMARY"),
                    markdown + "\n", cancellationToken).ConfigureAwait(false))
            {
                throw new InvalidOperationException("Appending to the GitHub Step Summary would exceed the 1 MiB file size limit.");
            }

            return;
        }

        if (Current == BuildSystem.AzurePipelines)
        {
            // Keep the file until the agent has consumed the upload command.
            var directory = variables.Get("AGENT_TEMPDIRECTORY") ?? files.GetTempPath();
            var path = Path.GetFullPath(Path.Combine(directory, $"modular-pipelines-summary-{Guid.NewGuid():N}.md"));
            await files.WriteAllTextAsync(path, Mask(markdown) + "\n", cancellationToken).ConfigureAwait(false);
            WriteCommand($"##vso[task.uploadsummary]{AzurePipelinesFormatter.EscapeLoggingCommandValue(path)}", cancellationToken);
            return;
        }

        throw Unsupported();
    }

    /// <inheritdoc />
    public Task AnnotateAsync(BuildAnnotationLevel level, string message, string? file = null, int? line = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        if (file is not null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(file);
        }

        if (!Enum.IsDefined(level))
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        if (line is <= 0 || (line is not null && string.IsNullOrWhiteSpace(file)))
        {
            throw new ArgumentException("A line number must be positive and requires a file name.", nameof(line));
        }

        cancellationToken.ThrowIfCancellationRequested();
        if (IsCI)
        {
            WriteCommand(formatters.GetFormatter(Current).GetAnnotationCommand(level, Mask(message),
                file is null ? null : Mask(file), line), cancellationToken);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SetBuildNumberAsync(string buildNumber, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(buildNumber);
        cancellationToken.ThrowIfCancellationRequested();
        if (IsCI)
        {
            WriteCommand(formatters.GetFormatter(Current).GetBuildNumberCommand(Mask(buildNumber)), cancellationToken);
        }

        return Task.CompletedTask;
    }

    private string Mask(string value) => obfuscator.Obfuscate(value, null);

    private string GetEnvironmentFile(string name) => variables.Get(name) is { Length: > 0 } path
        ? path
        : throw new InvalidOperationException($"The build system did not supply {name}.");

    private NotSupportedException Unsupported() => new($"This operation is not supported by {Current}.");

    private void WriteCommand(string? command, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        commandWriter.WriteLine(command ?? throw Unsupported());
    }
}
