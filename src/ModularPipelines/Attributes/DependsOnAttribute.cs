using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

/// <summary>
/// Declares a dependency on another module. The current module will not execute until the dependency has completed.
/// </summary>
/// <remarks>
/// <para>
/// By default, dependencies are <b>required</b>. Required dependencies are automatically registered
/// if not explicitly added to the pipeline. This ensures all dependencies are always present.
/// </para>
/// <para>
/// Use <see cref="Optional"/> = <c>true</c> for dependencies that may or may not be present.
/// Optional dependencies are not auto-registered and won't cause validation errors if missing.
/// </para>
/// <example>
/// <code>
/// // Required dependency - Module1 will be auto-registered if not present
/// [DependsOn&lt;Module1&gt;]
/// public class Module2 : Module&lt;string&gt; { }
///
/// // Optional dependency - Module1 won't be auto-registered
/// [DependsOn&lt;Module1&gt;(Optional = true)]
/// public class Module3 : Module&lt;string&gt;
/// {
///     protected override async Task&lt;string?&gt; ExecuteAsync(IModuleContext context, CancellationToken ct)
///     {
///         // Use GetModuleIfRegistered for optional dependencies
///         var module1 = context.GetModuleIfRegistered&lt;Module1&gt;();
///         if (module1 != null)
///         {
///             var result = await module1;
///             return result.Value;
///         }
///         return "Module1 not available";
///     }
/// }
/// </code>
/// </example>
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class.
    /// </summary>
    /// <param name="type">The type of module this module depends on.</param>
    /// <exception cref="InvalidModuleTypeException">Thrown when the type does not implement <see cref="IModule"/>.</exception>
    [Obsolete("Use the generic DependsOnAttribute<TModule> instead for compile-time type safety. This constructor will be removed in a future version.")]
    public DependsOnAttribute(Type type)
    {
        if (!type.IsAssignableTo(typeof(IModule)))
        {
            throw new InvalidModuleTypeException(type);
        }

        Type = type;
    }

    /// <summary>
    /// Internal constructor for use by the generic DependsOnAttribute to bypass the obsolete warning.
    /// </summary>
    internal DependsOnAttribute(Type type, bool skipValidation)
    {
        Type = type;
    }

    /// <summary>
    /// Gets the type of module this module depends on.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets or sets whether this dependency is optional.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When <c>false</c> (default), the dependency is <b>required</b>:
    /// </para>
    /// <list type="bullet">
    /// <item>The dependency module will be <b>auto-registered</b> if not explicitly added to the pipeline</item>
    /// <item>Use <c>GetModule&lt;T&gt;()</c> to access the dependency - it is guaranteed to be present</item>
    /// </list>
    /// <para>
    /// When <c>true</c>, the dependency is <b>optional</b>:
    /// </para>
    /// <list type="bullet">
    /// <item>The dependency module will <b>not</b> be auto-registered</item>
    /// <item>Use <c>GetModuleIfRegistered&lt;T&gt;()</c> to safely check if the dependency exists</item>
    /// <item>Useful when using category filters where dependencies may be excluded</item>
    /// </list>
    /// </remarks>
    /// <value>
    /// <c>true</c> if the dependency is optional; <c>false</c> if the dependency is required. Default is <c>false</c>.
    /// </value>
    public bool Optional { get; set; } = false;
}

/// <summary>
/// Declares a dependency on another module of type <typeparamref name="TModule"/>.
/// The current module will not execute until the dependency has completed.
/// </summary>
/// <typeparam name="TModule">The type of module this module depends on.</typeparam>
/// <remarks>
/// <para>
/// By default, dependencies are <b>required</b>. Required dependencies are automatically registered
/// if not explicitly added to the pipeline. This ensures all dependencies are always present.
/// </para>
/// <para>
/// Use <see cref="DependsOnAttribute.Optional"/> = <c>true</c> for dependencies that may or may not be present.
/// Optional dependencies are not auto-registered and won't cause validation errors if missing.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Required dependency - Module1 will be auto-registered if not present
/// [DependsOn&lt;Module1&gt;]
/// public class Module2 : Module&lt;string&gt; { }
///
/// // Optional dependency - Module1 won't be auto-registered
/// [DependsOn&lt;Module1&gt;(Optional = true)]
/// public class Module3 : Module&lt;string&gt; { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute<TModule> : DependsOnAttribute
    where TModule : IModule
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DependsOnAttribute{TModule}"/> class.
    /// </summary>
    public DependsOnAttribute() : base(typeof(TModule), skipValidation: true)
    {
    }
}
