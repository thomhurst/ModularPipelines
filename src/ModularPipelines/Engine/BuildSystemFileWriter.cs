using System.Text;
using ModularPipelines.FileSystem;
using ModularPipelines.Secrets;

namespace ModularPipelines.Engine;

/// <summary>
/// Serializes CI environment-file writes, including automatic and user GitHub summaries.
/// </summary>
internal sealed class BuildSystemFileWriter(IFileSystemProvider files, ISecretObfuscator obfuscator) : IDisposable
{
    private const long MaxSummaryBytes = 1024 * 1024;
    private readonly SemaphoreSlim _gate = new(1, 1);

    internal async Task AppendOutputAsync(string path, string name, string value, CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var maskedValue = obfuscator.Obfuscate(value, null);
            string delimiter;
            do
            {
                delimiter = $"mp_{Guid.NewGuid():N}";
            }
            while (maskedValue.Contains(delimiter, StringComparison.Ordinal));

            // A leading newline also separates us from a pre-existing unterminated record.
            await files.AppendAllTextAsync(path, $"\n{name}<<{delimiter}\n{maskedValue}\n{delimiter}\n", cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            _gate.Release();
        }
    }

    internal async Task<bool> AppendGitHubSummaryAsync(
        string path, string markdown, CancellationToken cancellationToken, string? optionalPrefix = null)
    {
        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var currentBytes = files.FileExists(path) ? files.GetFileLength(path) : 0;
            var contents = obfuscator.Obfuscate(markdown, null);
            var contentBytes = Encoding.UTF8.GetByteCount(contents);
            if (currentBytes + contentBytes > MaxSummaryBytes)
            {
                return false;
            }

            if (optionalPrefix is not null)
            {
                var prefix = obfuscator.Obfuscate(optionalPrefix, null);
                if (currentBytes + contentBytes + Encoding.UTF8.GetByteCount(prefix) <= MaxSummaryBytes)
                {
                    contents = prefix + contents;
                }
            }

            await files.AppendAllTextAsync(path, contents, cancellationToken).ConfigureAwait(false);
            return true;
        }
        finally
        {
            _gate.Release();
        }
    }

    public void Dispose() => _gate.Dispose();
}
