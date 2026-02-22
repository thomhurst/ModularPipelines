using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Capabilities;

internal static class CapabilityMatcher
{
    /// <summary>
    /// Checks if a worker can execute a module assignment based on capabilities.
    /// </summary>
    public static bool CanExecute(ModuleAssignment assignment, WorkerRegistration worker)
    {
        if (assignment.RequiredCapabilities.Count == 0)
        {
            return true;
        }

        return assignment.RequiredCapabilities.All(
            required => worker.Capabilities.Contains(required, StringComparer.OrdinalIgnoreCase));
    }
}
