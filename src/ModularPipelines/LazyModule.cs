using ModularPipelines.Models;

namespace ModularPipelines;

public class LazyModule<T>
{
    private readonly Lazy<Task<ModuleResult<T>>> _lazy;
    private readonly TaskCompletionSource<ModuleResult<T>> _tcs = new();

    public LazyModule(Func<Task<ModuleResult<T>>> func)
    {
        _lazy = new(async () =>
        {
            try
            {
                var result = await func();
                _tcs.SetResult(result);
                return result;
            }
            catch (Exception e)
            {
                _tcs.SetException(e);
                throw;
            }
        }, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public async Task Start()
    {
        await _lazy.Value;
    }

    public Task<ModuleResult<T>> GetTask() => _tcs.Task;
}