namespace ModularPipelines.Engine;

public interface IModuleEstimatedTimeProvider
{
    Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType);
    Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration);
    
    Task<TimeSpan> GetSubModuleEstimatedTimeAsync(Type moduleType, string subModuleName);
    Task SaveSubModuleTimeAsync(Type moduleType, string subModuleName, TimeSpan duration);
}