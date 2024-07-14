namespace ModularPipelines.Logging;

public interface IAfterPipelineLogger
{
    void LogOnPipelineEnd(string value);

    string GetOutput();
    
    internal void WriteLogs();
}