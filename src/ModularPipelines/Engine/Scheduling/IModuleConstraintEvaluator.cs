namespace ModularPipelines.Engine.Scheduling;

/// <summary>
/// Responsible for evaluating module execution constraints such as
/// NotInParallel attributes and lock key conflicts.
/// </summary>
internal interface IModuleConstraintEvaluator
{
    /// <summary>
    /// Determines if a module can be queued for execution given the currently active modules.
    /// This is called during the scheduling phase before a module is put in the queue.
    /// </summary>
    /// <param name="moduleState">The module to evaluate.</param>
    /// <param name="activeModules">Modules that are currently queued or executing.</param>
    /// <returns>True if the module can be queued, false if constraints prevent it.</returns>
    bool CanQueue(ModuleState moduleState, IEnumerable<ModuleState> activeModules);

    /// <summary>
    /// Determines if a module can start execution given the currently executing modules.
    /// This is called at the execution boundary, just before a module begins running.
    /// </summary>
    /// <param name="moduleState">The module to evaluate.</param>
    /// <param name="executingModules">Modules that are currently executing.</param>
    /// <returns>True if the module can start execution, false if constraints prevent it.</returns>
    bool CanStartExecution(ModuleState moduleState, IEnumerable<ModuleState> executingModules);
}
