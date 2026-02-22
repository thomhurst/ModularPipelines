namespace ModularPipelines.Attributes;

/// <summary>
/// Declares that a module should be expanded into multiple instances at registration time,
/// one for each target value. Each expanded instance gets a RequiresCapability for its target.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class MatrixTargetAttribute : Attribute
{
    public MatrixTargetAttribute(params string[] targets)
    {
        Targets = targets;
    }

    public string[] Targets { get; }
}
