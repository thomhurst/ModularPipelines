using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Context provided to modules during execution, extending pipeline context with module-specific capabilities.
/// </summary>
/// <remarks>
/// <para>
/// This is the primary interface for module developers. It provides access to all pipeline capabilities
/// including DI, configuration, file system, commands, serialization, and module results.
/// </para>
/// <para><b>Thread Safety:</b></para>
/// <para>
/// <see cref="IModuleContext"/> instances are thread-safe for concurrent access.
/// Multiple threads within a module may safely access context services simultaneously.
/// This is essential since modules often perform parallel operations internally.
/// </para>
/// <list type="table">
/// <listheader><term>Component</term><description>Thread Safety</description></listheader>
/// <item><term>Logger</term><description>Thread-safe for all logging operations</description></item>
/// <item><term>FileSystem reads</term><description>Thread-safe for concurrent read operations</description></item>
/// <item><term>FileSystem writes</term><description>Safe, but coordinate access when multiple threads write to the same file</description></item>
/// <item><term>Environment</term><description>Thread-safe (provides read-only access to environment information)</description></item>
/// <item><term>Command execution</term><description>Thread-safe; multiple commands can execute concurrently</description></item>
/// <item><term>Service resolution</term><description>Thread-safe via the underlying DI container</description></item>
/// <item><term>Configuration</term><description>Thread-safe for read access</description></item>
/// </list>
/// <para>
/// <b>Note on Module Results:</b> Results obtained via <see cref="GetModule{TModule}"/> have thread safety
/// determined by the result type itself. The framework ensures safe delivery of the result, but if multiple
/// threads access a mutable result object, thread safety is the consumer's responsibility.
/// Use immutable result types (e.g., records) for inherently safe concurrent access.
/// </para>
/// <para>
/// <b>Example - Safe Concurrent Access:</b>
/// </para>
/// <code>
/// protected override async Task&lt;MyResult&gt; ExecuteAsync(IModuleContext context, CancellationToken token)
/// {
///     // Safe: Multiple parallel tasks accessing the same context
///     var tasks = files.Select(async file =&gt;
///     {
///         context.Logger.LogInformation("Processing {File}", file);  // Thread-safe
///         var content = await context.FileSystem.ReadTextAsync(file); // Thread-safe
///         return ProcessFile(content);
///     });
///
///     var results = await Task.WhenAll(tasks);
///     return new MyResult(results);
/// }
/// </code>
/// </remarks>
public interface IModuleContext : IPipelineContext
{
    /// <summary>
    /// Gets a module instance that can be awaited to retrieve its result.
    /// </summary>
    /// <typeparam name="TModule">
    /// The module type to retrieve. Must inherit from <see cref="Module{T}"/>.
    /// </typeparam>
    /// <returns>
    /// The module instance, which can be awaited to get its <see cref="ModuleResult{T}"/>.
    /// The result type is automatically inferred from the module's base class.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <b>Simplified API:</b> This method requires only a single type parameter.
    /// The result type is inferred from the module's class hierarchy when you await the result.
    /// </para>
    /// <para>
    /// <b>Example usage:</b>
    /// </para>
    /// <code>
    /// [DependsOn&lt;BuildModule&gt;]
    /// public class DeployModule : Module&lt;DeployResult&gt;
    /// {
    ///     protected override async Task&lt;DeployResult&gt; ExecuteAsync(
    ///         IModuleContext context, CancellationToken cancellationToken)
    ///     {
    ///         // Single type parameter - result type inferred from BuildModule : Module&lt;BuildOutput&gt;
    ///         var buildResult = await context.GetModule&lt;BuildModule&gt;();
    ///         // buildResult is ModuleResult&lt;BuildOutput?&gt;
    ///
    ///         if (buildResult.IsSuccess)
    ///         {
    ///             var artifactPath = buildResult.ValueOrDefault!.ArtifactPath;
    ///             return await Deploy(artifactPath);
    ///         }
    ///
    ///         return new DeployResult { Skipped = true };
    ///     }
    /// }
    /// </code>
    /// <para>
    /// <b>How it works:</b> The module class hierarchy encodes the result type.
    /// When <c>BuildModule : Module&lt;BuildOutput&gt;</c>, awaiting the module
    /// returns <c>ModuleResult&lt;BuildOutput?&gt;</c> because <see cref="Module{T}"/>
    /// implements <see cref="Module{T}.GetAwaiter"/>.
    /// </para>
    /// <para>
    /// <b>Important:</b> Always declare dependencies using <see cref="Attributes.DependsOnAttribute{T}"/>
    /// to ensure the dependent module has completed before calling <c>GetModule</c>.
    /// </para>
    /// </remarks>
    /// <exception cref="Exceptions.ModuleNotRegisteredException">
    /// Thrown if the specified module was not registered with the pipeline.
    /// Ensure the module is registered via <c>AddModule&lt;TModule&gt;()</c> in your pipeline configuration.
    /// </exception>
    /// <exception cref="Exceptions.ModuleReferencingSelfException">
    /// Thrown if a module attempts to retrieve itself. A module cannot depend on its own result.
    /// </exception>
    /// <seealso cref="Module{T}"/>
    /// <seealso cref="Attributes.DependsOnAttribute{T}"/>
    /// <seealso cref="GetModuleIfRegistered{TModule}"/>
    TModule GetModule<TModule>()
        where TModule : class, IModule;

    /// <summary>
    /// Gets a module instance if the module is registered, or <c>null</c> if not registered.
    /// The returned module can be awaited to retrieve its result.
    /// </summary>
    /// <typeparam name="TModule">
    /// The module type to retrieve. Must inherit from <see cref="Module{T}"/>.
    /// </typeparam>
    /// <returns>
    /// The module instance if registered, or <c>null</c> if the module was not registered.
    /// The module can be awaited to get its <see cref="ModuleResult{T}"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <b>Simplified API:</b> This method requires only a single type parameter.
    /// The result type is inferred from the module's class hierarchy when you await the result.
    /// </para>
    /// <para>
    /// <b>When to use this method:</b>
    /// </para>
    /// <para>
    /// Use <c>GetModuleIfRegistered</c> when you want to optionally consume a module's result
    /// without requiring that module to be registered. This is useful for:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Optional integrations where a module may or may not be present</description></item>
    /// <item><description>Plugin architectures where modules are conditionally registered</description></item>
    /// <item><description>Feature flags that control which modules run</description></item>
    /// </list>
    /// <para>
    /// <b>Example usage:</b>
    /// </para>
    /// <code>
    /// [DependsOn&lt;BuildModule&gt;(Optional = true)]
    /// public class DeployModule : Module&lt;DeployResult&gt;
    /// {
    ///     protected override async Task&lt;DeployResult&gt; ExecuteAsync(
    ///         IModuleContext context, CancellationToken cancellationToken)
    ///     {
    ///         // Single type parameter - returns null if not registered
    ///         var buildModule = context.GetModuleIfRegistered&lt;BuildModule&gt;();
    ///
    ///         if (buildModule is not null)
    ///         {
    ///             var buildResult = await buildModule;
    ///             // Use build result...
    ///         }
    ///
    ///         return await Deploy();
    ///     }
    /// }
    /// </code>
    /// <para>
    /// <b>Important:</b> If you use this method with a module that may be registered, consider using
    /// <see cref="Attributes.DependsOnAttribute.Optional"/> on your dependency attribute
    /// to properly handle the optional dependency in the execution graph.
    /// </para>
    /// </remarks>
    /// <seealso cref="GetModule{TModule}"/>
    /// <seealso cref="Module{T}"/>
    /// <seealso cref="Attributes.DependsOnAttribute{T}"/>
    TModule? GetModuleIfRegistered<TModule>()
        where TModule : class, IModule;

    /// <summary>
    /// Tracks a sub-operation within the current module for progress display.
    /// </summary>
    /// <typeparam name="T">The result type of the sub-operation.</typeparam>
    /// <param name="name">A descriptive name for the sub-operation.</param>
    /// <param name="action">The async operation to execute.</param>
    /// <returns>The result of the sub-operation.</returns>
    Task<T> SubModule<T>(string name, Func<Task<T>> action);

    /// <summary>
    /// Tracks a sub-operation within the current module for progress display.
    /// </summary>
    /// <param name="name">A descriptive name for the sub-operation.</param>
    /// <param name="action">The async operation to execute.</param>
    /// <returns>A task representing the sub-operation.</returns>
    Task SubModule(string name, Func<Task> action);
}
