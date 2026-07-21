using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Capabilities;

public static class CapabilityMatcher
{
    /// <summary>
    /// Checks if a worker can execute a module assignment based on capabilities.
    /// </summary>
    public static bool CanExecute(ModuleAssignment assignment, WorkerRegistration worker)
    {
        return CanExecute(assignment, worker.Capabilities);
    }

    /// <summary>
    /// Checks if the given capabilities satisfy a module assignment's requirements.
    /// </summary>
    public static bool CanExecute(ModuleAssignment assignment, IReadOnlySet<string> workerCapabilities)
    {
        if (assignment.RequiredCapabilities.Count == 0)
        {
            return true;
        }

        return assignment.RequiredCapabilities.All(
            required => workerCapabilities.Contains(required, StringComparer.OrdinalIgnoreCase));
    }
}
