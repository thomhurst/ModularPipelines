namespace ModularPipelines.Logging;

public interface IAfterPipelineLogger
{
    void LogOnPipelineEnd(string value);
    
    internal void WriteLogs();
}