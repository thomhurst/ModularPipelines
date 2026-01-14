using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.DependencyInjection;

/// <summary>
/// Fluent builder for configuring module registration with metadata.
/// </summary>
/// <remarks>
/// <para>
/// This builder is returned from module registration methods and allows:
/// </para>
/// <list type="bullet">
/// <item>Chaining additional module registrations using extension methods from <see cref="Extensions.ModuleRegistrationBuilderExtensions"/></item>
/// <item>Adding tags to modules for filtering and grouping</item>
/// <item>Setting categories for conditional execution</item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// // Chained registration with metadata
/// services.AddModule&lt;BuildModule&gt;()
///     .WithCategory("Core")
///     .WithTags("build", "compile")
///     .AddModule&lt;TestModule&gt;()
///     .WithCategory("Testing");
/// </code>
/// </example>
public interface IModuleRegistrationBuilder
{
    /// <summary>
    /// Gets the service collection for continued registration.
    /// </summary>
    /// <remarks>
    /// This property provides access to the underlying <see cref="IServiceCollection"/> for
    /// registering additional services or using other service collection extension methods.
    /// </remarks>
    IServiceCollection Services { get; }

    /// <summary>
    /// Adds tags to the module for filtering and grouping.
    /// </summary>
    /// <param name="tags">One or more tags to associate with the module.</param>
    /// <returns>The builder for chaining additional configuration.</returns>
    /// <remarks>
    /// <para>
    /// Tags can be used to group modules and enable conditional execution or dependency
    /// declarations based on tags. Multiple calls to <see cref="WithTags"/> are cumulative.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// services.AddModule&lt;BuildModule&gt;()
    ///     .WithTags("build", "core")
    ///     .WithTags("ci");  // Adds to existing tags
    /// </code>
    /// </example>
    IModuleRegistrationBuilder WithTags(params string[] tags);

    /// <summary>
    /// Sets the category of the module for conditional execution.
    /// </summary>
    /// <param name="category">The category to assign to the module.</param>
    /// <returns>The builder for chaining additional configuration.</returns>
    /// <remarks>
    /// <para>
    /// Categories can be used with <see cref="Host.PipelineHostBuilder.RunCategories"/> and
    /// <see cref="Host.PipelineHostBuilder.IgnoreCategories"/> to control which modules execute.
    /// </para>
    /// <para>
    /// If called multiple times, the last category wins.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// services.AddModule&lt;DeployModule&gt;()
    ///     .WithCategory("Production");
    ///
    /// // Later, run only production modules:
    /// builder.RunCategories("Production");
    /// </code>
    /// </example>
    IModuleRegistrationBuilder WithCategory(string category);
}
