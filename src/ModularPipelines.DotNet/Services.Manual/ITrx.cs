using ModularPipelines.FileSystem;

namespace ModularPipelines.DotNet.Services;

public interface ITrx
{
    Task<DotNetTestResult> ParseTrxFile(FilePath file);
}