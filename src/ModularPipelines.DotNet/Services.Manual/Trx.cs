using ModularPipelines.FileSystem;

namespace ModularPipelines.DotNet.Services;

internal class Trx(ITrxParser trxParser) : ITrx
{
    public async Task<DotNetTestResult> ParseTrxFile(FilePath file)
    {
        var contents = await file.ReadAsync();

        return trxParser.ParseTrxContents(contents);
    }
}