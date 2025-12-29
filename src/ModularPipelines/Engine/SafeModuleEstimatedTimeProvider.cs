using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal class SafeModuleEstimatedTimeProvider : ISafeModuleEstimatedTimeProvider, IScopeDisposer
{
    private readonly IModuleEstimatedTimeProvider _moduleEstimatedTimeProvider;
    private readonly ILogger _logger;
    private readonly List<IServiceScope> _scopes = new();

    public SafeModuleEstimatedTimeProvider(IModuleEstimatedTimeProvider moduleEstimatedTimeProvider, IServiceProvider serviceProvider)
    {
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
        var scope = serviceProvider.CreateScope();
        _scopes.Add(scope);
        _logger = scope.ServiceProvider.GetRequiredService<IModuleLoggerProvider>().GetLogger();
    }

    public async Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
    {
        try
        {
            return await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(moduleType).ConfigureAwait(false);
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
            await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(moduleType, duration).ConfigureAwait(false);
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
            return await _moduleEstimatedTimeProvider.GetSubModuleEstimatedTimesAsync(moduleType).ConfigureAwait(false) ?? new List<SubModuleEstimation>();
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
            await _moduleEstimatedTimeProvider.SaveSubModuleTimeAsync(moduleType, subModuleEstimation).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error saving submodule execution time for {Module}", moduleType.Name);
        }
    }

    public IEnumerable<IServiceScope> GetScopes()
    {
        return _scopes;
    }
}