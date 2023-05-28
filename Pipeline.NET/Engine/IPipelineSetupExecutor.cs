namespace Pipeline.NET.Engine;

public interface IPipelineSetupExecutor
{
    Task OnStartAsync();
    Task OnEndAsync();
}