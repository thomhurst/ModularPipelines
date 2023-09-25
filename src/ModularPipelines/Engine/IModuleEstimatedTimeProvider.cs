using ModularPipelines.Models;

namespace ModularPipelines.Engine;

public interface IModuleEstimatedTimeProvider
{
    Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType);

    Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration);

    Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType);

    Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation);
}