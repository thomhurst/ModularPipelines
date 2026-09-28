using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines;

/// <summary>
/// Declares a dependency on every registered module that is, inherits from, or implements a base type.
/// </summary>
/// <remarks>
/// <para>
/// The dependencies are selected from the registered modules when the pipeline starts, so modules added
/// later by other registrations are included. The declaring module never depends on itself.
/// Selected dependencies are required, but they are not auto-registered: only registered modules are selected.
/// </para>
/// <para>
/// Use this overload from .NET languages that cannot apply generic attributes, such as F#.
/// C# callers should prefer <see cref="DependsOnAllModulesInheritingFromAttribute{TModule}"/>.
/// Open generic base types, such as <c>typeof(BuildModule&lt;&gt;)</c>, match every closed construction.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// [DependsOnAllModulesInheritingFrom(typeof(RunUnitTestModule))]
/// public class RunAllUnitTestsModule : Module&lt;None&gt; { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public sealed class DependsOnAllModulesInheritingFromAttribute : DependsOnBaseAttribute, IPlanningSafe, IInheritanceDependencySelector
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DependsOnAllModulesInheritingFromAttribute"/> class.
    /// </summary>
    /// <param name="type">The module base type or interface that selected modules derive from or implement.</param>
    /// <exception cref="InvalidModuleTypeException">Thrown when the type does not implement <see cref="IModule"/>.</exception>
    public DependsOnAllModulesInheritingFromAttribute(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!type.IsAssignableTo(typeof(IModule)))
        {
            throw new InvalidModuleTypeException(type);
        }

        Type = type;
    }

    /// <summary>
    /// Gets the module base type or interface that selected modules derive from or implement.
    /// </summary>
    public Type Type { get; }

    /// <inheritdoc />
    public override bool ShouldDependOn(Type candidateModule, IDependencyContext context) =>
        candidateModule.IsOrInheritsFrom(Type);
}

/// <summary>
/// Declares a dependency on every registered module that is, inherits from, or implements
/// <typeparamref name="TModule"/>.
/// </summary>
/// <typeparam name="TModule">The module base type or interface that selected modules derive from or implement.</typeparam>
/// <remarks>
/// See <see cref="DependsOnAllModulesInheritingFromAttribute"/> for selection semantics.
/// </remarks>
/// <example>
/// <code>
/// [DependsOnAllModulesInheritingFrom&lt;RunUnitTestModule&gt;]
/// public class RunAllUnitTestsModule : Module&lt;None&gt; { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public sealed class DependsOnAllModulesInheritingFromAttribute<TModule> : DependsOnBaseAttribute, IPlanningSafe, IInheritanceDependencySelector
    where TModule : IModule
{
    /// <summary>
    /// Gets the module base type or interface that selected modules derive from or implement.
    /// </summary>
    public Type Type => typeof(TModule);

    /// <inheritdoc />
    public override bool ShouldDependOn(Type candidateModule, IDependencyContext context) =>
        candidateModule.IsOrInheritsFrom(typeof(TModule));
}

/// <summary>
/// A dependency selector that needs no module metadata, so it can run before the metadata registry exists.
/// </summary>
internal interface IInheritanceDependencySelector
{
    Type Type { get; }
}
