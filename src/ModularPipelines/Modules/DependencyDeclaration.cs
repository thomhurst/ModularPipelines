using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

/// <summary>
/// Implementation of <see cref="IDependencyDeclaration"/> for collecting programmatic dependencies.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is not thread-safe. It is designed to be used
/// during the synchronous <see cref="Module{T}.DeclareDependencies"/> method call
/// and should not be accessed from multiple threads.
/// </para>
/// </remarks>
internal sealed class DependencyDeclaration : IDependencyDeclaration
{
    private readonly List<DeclaredDependency> _dependencies = new();

    /// <summary>
    /// Gets the declared dependencies.
    /// </summary>
    public IReadOnlyList<DeclaredDependency> Dependencies => _dependencies;

    /// <inheritdoc />
    public IDependencyDeclaration DependsOn<TModule>() where TModule : IModule
    {
        return DependsOn(typeof(TModule));
    }

    /// <inheritdoc />
    public IDependencyDeclaration DependsOn(Type moduleType)
    {
        ValidateModuleType(moduleType);
        _dependencies.Add(DeclaredDependency.Required(moduleType));
        return this;
    }

    /// <inheritdoc />
    public IDependencyDeclaration DependsOnOptional<TModule>() where TModule : IModule
    {
        return DependsOnOptional(typeof(TModule));
    }

    /// <inheritdoc />
    public IDependencyDeclaration DependsOnOptional(Type moduleType)
    {
        ValidateModuleType(moduleType);
        _dependencies.Add(DeclaredDependency.Optional(moduleType));
        return this;
    }

    /// <inheritdoc />
    public IDependencyDeclaration DependsOnIf<TModule>(bool condition) where TModule : IModule
    {
        return DependsOnIf(typeof(TModule), condition);
    }

    /// <inheritdoc />
    public IDependencyDeclaration DependsOnIf<TModule>(Func<bool> predicate) where TModule : IModule
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return DependsOnIf(typeof(TModule), predicate());
    }

    /// <inheritdoc />
    public IDependencyDeclaration DependsOnIf(Type moduleType, bool condition)
    {
        if (!condition)
        {
            return this;
        }

        ValidateModuleType(moduleType);
        _dependencies.Add(DeclaredDependency.Conditional(moduleType));
        return this;
    }

    /// <inheritdoc />
    public IDependencyDeclaration DependsOnIf(Type moduleType, Func<bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return DependsOnIf(moduleType, predicate());
    }

    /// <inheritdoc />
    public IDependencyDeclaration DependsOnLazy<TModule>() where TModule : IModule
    {
        return DependsOnLazy(typeof(TModule));
    }

    /// <inheritdoc />
    public IDependencyDeclaration DependsOnLazy(Type moduleType)
    {
        ValidateModuleType(moduleType);
        _dependencies.Add(DeclaredDependency.Lazy(moduleType));
        return this;
    }

    private static void ValidateModuleType(Type moduleType)
    {
        ArgumentNullException.ThrowIfNull(moduleType);

        if (!moduleType.IsAssignableTo(typeof(IModule)))
        {
            throw new ArgumentException(
                $"{moduleType.FullName} is not a Module (does not implement IModule)",
                nameof(moduleType));
        }
    }
}
