using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;
using ModularPipelines.Models;

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

    public async Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType)
    {
        try
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            return await _moduleEstimatedTimeProvider.GetSubModuleEstimatedTimesAsync(moduleType) ?? new List<SubModuleEstimation>();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error retrieving submodule estimated time for {Module}", moduleType.Name);
            return new List<SubModuleEstimation>();
        }
    }

    public async Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation)
    {
        try
        {
            await _moduleEstimatedTimeProvider.SaveSubModuleTimeAsync(moduleType, subModuleEstimation);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error saving submodule execution time for {Module}", moduleType.Name);
        }
    }
}