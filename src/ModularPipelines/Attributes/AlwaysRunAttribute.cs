using ModularPipelines.Models;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies that this module should always run, even if its dependencies fail.
/// By default, modules only run if all their dependencies succeed (ModuleRunType.OnSuccessfulDependencies).
/// With this attribute, the module will run regardless of dependency outcomes (ModuleRunType.AlwaysRun).
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AlwaysRunAttribute : Attribute
{
    /// <summary>
    /// Gets the module run type.
    /// </summary>
    public ModuleRunType RunType => ModuleRunType.AlwaysRun;
}
