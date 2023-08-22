using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;

namespace ModularPipelines.Engine;

internal class SafeModuleEstimatedTimeProvider : ISafeModuleEstimatedTimeProvider
{
    private readonly IModuleEstimatedTimeProvider _moduleEstimatedTimeProvider;
    private readonly ILogger _logger;

    public SafeModuleEstimatedTimeProvider(IModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
        _logger = moduleLoggerProvider.GetLogger();
    }

    public async Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
    {
        try
        {
            return await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(moduleType);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error retrieving module estimated time for {Module}", moduleType.Name);
            return TimeSpan.FromMinutes(2);
        }
    }

    public async Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
    {
        try
        {
            await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(moduleType, duration);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error saving module execution time for {Module}", moduleType.Name);
        }
    }

    public async Task<TimeSpan> GetSubModuleEstimatedTimeAsync(Type moduleType, string subModuleName)
    {
        try
        {
            return await _moduleEstimatedTimeProvider.GetSubModuleEstimatedTimeAsync(moduleType, subModuleName);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error retrieving submodule estimated time for {Module}", moduleType.Name);
            return TimeSpan.FromMinutes(2);
        }
    }

    public async Task SaveSubModuleTimeAsync(Type moduleType, string subModuleName, TimeSpan duration)
    {
        try
        {
            await _moduleEstimatedTimeProvider.SaveSubModuleTimeAsync(moduleType, subModuleName, duration);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error saving submodule execution time for {Module}", moduleType.Name);
        }
    }
}