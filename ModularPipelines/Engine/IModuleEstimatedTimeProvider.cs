namespace ModularPipelines.Engine;

public interface IModuleEstimatedTimeProvider
{
    Task<TimeSpan> GetEstimatedTimeAsync(Type moduleType);
    Task SaveTimeAsync(Type moduleType, TimeSpan duration);
}