namespace ModularPipelines;

internal class WaitOrchestrator : IWaitOrchestrator
{
    private readonly TaskCompletionSource _taskCompletionSource = new();
    
    public Task WaitForFinish()
    {
        return _taskCompletionSource.Task;
    }

    public void NotifyFinish()
    {
        _taskCompletionSource.TrySetResult();
    }
}