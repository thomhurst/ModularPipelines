using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.DotNet.Services;

internal class Trx(ITrxParser trxParser) : ITrx
{
    public async Task<DotNetTestResult> ParseTrxFile(File file)
    {
        var contents = await file.ReadAsync();

        return trxParser.ParseTrxContents(contents);
    }
}