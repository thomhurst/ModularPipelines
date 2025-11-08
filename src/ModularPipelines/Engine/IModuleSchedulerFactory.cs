namespace ModularPipelines.Engine;

/// <summary>
/// Factory for creating module scheduler instances
/// </summary>
/// <remarks>
/// Using a factory pattern allows for better testability and dependency injection.
/// Tests can provide mock schedulers, and the factory encapsulates creation logic.
/// </remarks>
internal interface IModuleSchedulerFactory
{
    /// <summary>
    /// Creates a new module scheduler instance
    /// </summary>
    /// <returns>A configured module scheduler</returns>
    IModuleScheduler Create();
}
