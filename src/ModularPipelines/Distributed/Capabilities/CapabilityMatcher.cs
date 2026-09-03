using System.Runtime.CompilerServices;
using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Capabilities;

public static class CapabilityMatcher
{
    private static readonly ConditionalWeakTable<WorkerRegistration, IReadOnlySet<Capability>> CapabilitySets = new();

    /// <summary>
    /// Checks if a worker can execute a module assignment based on capabilities.
    /// </summary>
    public static bool CanExecute(ModuleAssignment assignment, WorkerRegistration worker)
    {
        if (assignment.RequiredCapabilities.Count == 0)
        {
            return true;
        }

        var capabilitySet = CapabilitySets.GetValue(
            worker,
            static registration => registration.Capabilities.ToHashSet());
        return assignment.RequiredCapabilities.All(capabilitySet.Contains);
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
