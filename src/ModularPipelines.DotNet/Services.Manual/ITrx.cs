using ModularPipelines.FileSystem;

namespace ModularPipelines.DotNet.Services;

public interface ITrx
{
    /// <summary>
    /// Reads a TRX file and parses its test results.
    /// </summary>
    /// <param name="file">The TRX file to read.</param>
    /// <param name="cancellationToken">The token used to cancel the file read.</param>
    /// <returns>The parsed test results.</returns>
    Task<DotNetTestResult> ParseTrxFileAsync(FilePath file, CancellationToken cancellationToken = default);
}
