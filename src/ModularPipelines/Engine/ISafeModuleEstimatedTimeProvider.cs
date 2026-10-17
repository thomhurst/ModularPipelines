using ModularPipelines.Reporting;

namespace ModularPipelines.Engine;

internal interface ISafeModuleEstimatedTimeProvider
{
    Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType, CancellationToken cancellationToken = default);

    Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration, CancellationToken cancellationToken = default);

    Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType, CancellationToken cancellationToken = default);

    Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation, CancellationToken cancellationToken = default);
}
