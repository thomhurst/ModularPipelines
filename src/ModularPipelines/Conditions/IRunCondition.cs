using ModularPipelines.Context;

namespace ModularPipelines.Conditions;

/// <summary>
/// Defines a condition that determines whether a module should run.
/// </summary>
/// <remarks>
/// Implement this interface to create custom run conditions.
/// Conditions are evaluated before module execution and can access
/// the pipeline context for environment checks, HTTP calls, etc.
/// </remarks>
/// <example>
/// <code>
/// public sealed class HasGitHubToken : IRunCondition
/// {
///     public Task&lt;bool&gt; EvaluateAsync(IPipelineHookContext context)
///         =&gt; Task.FromResult(!string.IsNullOrEmpty(
///             context.Environment.GetEnvironmentVariable("GITHUB_TOKEN")));
/// }
/// </code>
/// </example>
public interface IRunCondition
{
    /// <summary>
    /// Evaluates the condition asynchronously.
    /// </summary>
    /// <param name="context">The pipeline context for accessing environment, HTTP, etc.</param>
    /// <returns>
    /// A task that returns <c>true</c> if the condition is satisfied; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> EvaluateAsync(IPipelineHookContext context);
}
