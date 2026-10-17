using ModularPipelines.FileSystem;

namespace ModularPipelines.DotNet.Services;

internal class Trx(ITrxParser trxParser) : ITrx
{
    public async Task<DotNetTestResult> ParseTrxFileAsync(FilePath file, CancellationToken cancellationToken = default)
    {
        var contents = await file.ReadAsync(cancellationToken).ConfigureAwait(false);

        return trxParser.ParseTrxContents(contents);
    }
}
