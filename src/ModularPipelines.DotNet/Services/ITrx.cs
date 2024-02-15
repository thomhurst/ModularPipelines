using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.DotNet.Services;

public interface ITrx
{
    Task<DotNetTestResult> ParseTrxFile(File file);
}