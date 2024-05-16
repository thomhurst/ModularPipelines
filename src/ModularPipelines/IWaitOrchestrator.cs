namespace ModularPipelines;

internal interface IWaitOrchestrator
{
    Task WaitForFinish();
    
    void NotifyFinish();
}