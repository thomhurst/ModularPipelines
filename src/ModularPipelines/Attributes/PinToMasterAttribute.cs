namespace ModularPipelines.Attributes;

/// <summary>
/// Declares that a module should only execute on the master instance in distributed mode.
/// The master will never enqueue this module to the work queue.
/// In non-distributed mode, this attribute has no effect.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class PinToMasterAttribute : Attribute
{
}
