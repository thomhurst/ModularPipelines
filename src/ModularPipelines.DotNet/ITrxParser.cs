namespace ModularPipelines.DotNet;

public interface ITrxParser
{
    DotNetTestResult ParseTrxContents(string input);
}