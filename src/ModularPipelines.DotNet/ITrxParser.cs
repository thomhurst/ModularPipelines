namespace ModularPipelines.DotNet;

public interface ITrxParser
{
    DotNetTestResult ParseTestResult(string input);
}
