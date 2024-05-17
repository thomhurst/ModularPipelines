using ModularPipelines.Models;

namespace ModularPipelines;

internal class LazyModule<T>
{
    private readonly Func<Task<ModuleResult<T>>> _func;
    private readonly TaskCompletionSource<ModuleResult<T>> _tcs = new();

    public LazyModule(Func<Task<ModuleResult<T>>> func)
    {
        _func = func;
    }

    public async Task Start()
    {
        try
        {
            var result = await _func();
            _tcs.SetResult(result);
        }
        catch (Exception e)
        {
            _tcs.SetException(e);
            throw;
        }
    }

    public Task<ModuleResult<T>> GetTask() => _tcs.Task;
}