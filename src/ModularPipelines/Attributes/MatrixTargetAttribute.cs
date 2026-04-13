namespace ModularPipelines.Attributes;

/// <summary>
/// Declares that a module should be expanded into multiple instances at registration time,
/// one for each target value. Each expanded instance gets a RequiresCapability for its target.
/// </summary>
/// <remarks>
/// Not yet wired into the distributed executor. Reserved for future use.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
internal sealed class MatrixTargetAttribute : Attribute
{
    public MatrixTargetAttribute(params string[] targets)
    {
        Targets = targets;
    }

    public string[] Targets { get; }
}
