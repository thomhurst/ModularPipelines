using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Depend on all modules decorated with the specified attribute.
/// </summary>
/// <typeparam name="TAttribute">The attribute type that target modules must have.</typeparam>
/// <example>
/// <code>
/// [DependsOnModulesWithAttribute&lt;CriticalAttribute&gt;]
/// public class AfterCriticalModule : Module&lt;string&gt; { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class DependsOnModulesWithAttributeAttribute<TAttribute> : DependsOnBaseAttribute
    where TAttribute : Attribute
{
    /// <inheritdoc />
    public override bool ShouldDependOn(Type candidateModule, IDependencyContext context)
        => context.HasAttribute<TAttribute>(candidateModule);
}
