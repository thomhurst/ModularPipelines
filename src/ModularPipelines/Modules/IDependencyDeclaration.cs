namespace ModularPipelines.Modules;

/// <summary>
/// Interface for declaring module dependencies programmatically at runtime.
/// </summary>
/// <remarks>
/// <para>
/// This interface provides methods for declaring dependencies dynamically, complementing
/// the static <see cref="Attributes.DependsOnAttribute"/> approach.
/// </para>
/// <para>
/// Dependencies declared via this interface are combined with attribute-based dependencies.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ApiModule : Module&lt;ApiResult&gt;
/// {
///     protected override void DeclareDependencies(IDependencyDeclaration deps)
///     {
///         // Required dependency
///         deps.DependsOn&lt;DatabaseModule&gt;();
///
///         // Optional dependency (doesn't fail if not registered)
///         deps.DependsOnOptional&lt;CachingModule&gt;();
///
///         // Conditional dependency
///         if (Environment.IsProduction)
///         {
///             deps.DependsOn&lt;PerformanceMonitoringModule&gt;();
///         }
///
///         // Conditional with predicate
///         deps.DependsOnIf&lt;HeavyProcessingModule&gt;(() =&gt; ShouldRunHeavyProcessing);
///     }
/// }
/// </code>
/// </example>
public interface IDependencyDeclaration
{
    /// <summary>
    /// Declares a required dependency on the specified module type.
    /// </summary>
    /// <typeparam name="TModule">The type of module to depend on.</typeparam>
    /// <returns>This instance for method chaining.</returns>
    /// <exception cref="Exceptions.ModuleNotRegisteredException">
    /// Thrown during pipeline execution if the dependency is not registered.
    /// </exception>
    IDependencyDeclaration DependsOn<TModule>() where TModule : IModule;

    /// <summary>
    /// Declares a required dependency on the specified module type.
    /// </summary>
    /// <param name="moduleType">The type of module to depend on.</param>
    /// <returns>This instance for method chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the type does not implement <see cref="IModule"/>.
    /// </exception>
    /// <exception cref="Exceptions.ModuleNotRegisteredException">
    /// Thrown during pipeline execution if the dependency is not registered.
    /// </exception>
    IDependencyDeclaration DependsOn(Type moduleType);

    /// <summary>
    /// Declares an optional dependency on the specified module type.
    /// Unlike <see cref="DependsOn{TModule}"/>, this will not fail if the dependency is not registered.
    /// </summary>
    /// <typeparam name="TModule">The type of module to depend on.</typeparam>
    /// <returns>This instance for method chaining.</returns>
    IDependencyDeclaration DependsOnOptional<TModule>() where TModule : IModule;

    /// <summary>
    /// Declares an optional dependency on the specified module type.
    /// Unlike <see cref="DependsOn(Type)"/>, this will not fail if the dependency is not registered.
    /// </summary>
    /// <param name="moduleType">The type of module to depend on.</param>
    /// <returns>This instance for method chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the type does not implement <see cref="IModule"/>.
    /// </exception>
    IDependencyDeclaration DependsOnOptional(Type moduleType);

    /// <summary>
    /// Declares a conditional dependency that is only active if the condition is true.
    /// </summary>
    /// <typeparam name="TModule">The type of module to depend on.</typeparam>
    /// <param name="condition">The condition that determines whether the dependency is active.</param>
    /// <returns>This instance for method chaining.</returns>
    /// <remarks>
    /// If the condition is false, the dependency is not added at all.
    /// </remarks>
    IDependencyDeclaration DependsOnIf<TModule>(bool condition) where TModule : IModule;

    /// <summary>
    /// Declares a conditional dependency that is only active if the predicate returns true.
    /// </summary>
    /// <typeparam name="TModule">The type of module to depend on.</typeparam>
    /// <param name="predicate">The predicate that determines whether the dependency is active.</param>
    /// <returns>This instance for method chaining.</returns>
    /// <remarks>
    /// The predicate is evaluated immediately when this method is called.
    /// If it returns false, the dependency is not added at all.
    /// </remarks>
    IDependencyDeclaration DependsOnIf<TModule>(Func<bool> predicate) where TModule : IModule;

    /// <summary>
    /// Declares a conditional dependency that is only active if the condition is true.
    /// </summary>
    /// <param name="moduleType">The type of module to depend on.</param>
    /// <param name="condition">The condition that determines whether the dependency is active.</param>
    /// <returns>This instance for method chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the type does not implement <see cref="IModule"/>.
    /// </exception>
    IDependencyDeclaration DependsOnIf(Type moduleType, bool condition);

    /// <summary>
    /// Declares a conditional dependency that is only active if the predicate returns true.
    /// </summary>
    /// <param name="moduleType">The type of module to depend on.</param>
    /// <param name="predicate">The predicate that determines whether the dependency is active.</param>
    /// <returns>This instance for method chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the type does not implement <see cref="IModule"/>.
    /// </exception>
    IDependencyDeclaration DependsOnIf(Type moduleType, Func<bool> predicate);

    /// <summary>
    /// Declares a lazy dependency - an optional dependency intended to be awaited on-demand.
    /// </summary>
    /// <typeparam name="TModule">The type of module to depend on.</typeparam>
    /// <returns>This instance for method chaining.</returns>
    /// <remarks>
    /// <para>
    /// Lazy dependencies behave the same as <see cref="DependsOnOptional{TModule}"/> for
    /// dependency resolution purposes - the dependency is optional and will not fail if not
    /// registered. This is a semantic marker to indicate intent that the dependency may be
    /// awaited on-demand rather than required upfront.
    /// </para>
    /// <para>
    /// <strong>Important:</strong> The lazy module will still execute during normal pipeline
    /// scheduling if it is registered. It does NOT defer execution until explicitly awaited.
    /// The "lazy" designation indicates the dependency relationship is optional and the result
    /// may be consumed on-demand, but does not affect when the module runs.
    /// </para>
    /// <para>
    /// Use this to express intent for optional processing that may or may not be awaited.
    /// </para>
    /// </remarks>
    IDependencyDeclaration DependsOnLazy<TModule>() where TModule : IModule;

    /// <summary>
    /// Declares a lazy dependency - an optional dependency intended to be awaited on-demand.
    /// </summary>
    /// <param name="moduleType">The type of module to depend on.</param>
    /// <returns>This instance for method chaining.</returns>
    /// <remarks>
    /// <para>
    /// Lazy dependencies behave the same as <see cref="DependsOnOptional(Type)"/> for
    /// dependency resolution purposes. See <see cref="DependsOnLazy{TModule}"/> for full details.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown if the type does not implement <see cref="IModule"/>.
    /// </exception>
    IDependencyDeclaration DependsOnLazy(Type moduleType);
}
