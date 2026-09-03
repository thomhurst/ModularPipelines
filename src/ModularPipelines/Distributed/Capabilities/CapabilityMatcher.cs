using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Capabilities;

public static class CapabilityMatcher
{
    /// <summary>
    /// Checks if a worker can execute a module assignment based on capabilities.
    /// </summary>
    public static bool CanExecute(ModuleAssignment assignment, WorkerRegistration worker)
    {
        return assignment.RequiredCapabilities.All(worker.Capabilities.Contains);
    }

    /// <summary>
    /// Checks if the given capabilities satisfy a module assignment's requirements.
    /// </summary>
    public static bool CanExecute(ModuleAssignment assignment, IReadOnlySet<Capability> workerCapabilities)
    {
        if (assignment.RequiredCapabilities.Count == 0)
        {
            return true;
        }

        return assignment.RequiredCapabilities.All(workerCapabilities.Contains);
    }
}
