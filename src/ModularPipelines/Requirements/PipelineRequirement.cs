using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

/// <summary>
/// Base class for pipeline requirements that simplifies implementation
/// by providing convenient factory methods for creating requirement decisions.
/// </summary>
/// <remarks>
/// <para>
/// This abstract base class provides a simpler pattern for implementing requirements.
/// Instead of directly returning <see cref="RequirementDecision"/> objects,
/// derived classes can use the protected helper methods.
/// </para>
/// <para><b>Simple Implementation Example:</b></para>
/// <code>
/// public class HasDotNetSdkRequirement : PipelineRequirement
/// {
///     public override async Task&lt;RequirementDecision&gt; MustAsync(IPipelineHookContext context)
///     {
///         var result = await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet", "--version"));
///         return result.ExitCode == 0 ? Pass() : Fail(".NET SDK is not installed");
///     }
/// }
/// </code>
/// <para><b>Synchronous Implementation Example:</b></para>
/// <code>
/// public class Is64BitProcessRequirement : PipelineRequirement
/// {
///     protected override RequirementDecision Must(IPipelineHookContext context)
///         =&gt; Environment.Is64BitProcess ? Pass() : Fail("64-bit process required");
/// }
/// </code>
/// </remarks>
public abstract class PipelineRequirement : IPipelineRequirement
{
    /// <summary>
    /// Gets the order in which this requirement should be evaluated relative to other requirements.
    /// Lower values are evaluated first. Default is 0.
    /// </summary>
    /// <remarks>
    /// Override this property to control the evaluation order of requirements.
    /// Requirements with lower order values are checked before those with higher values.
    /// Requirements with the same order value may be evaluated in parallel.
    /// </remarks>
    public virtual int Order => 0;

    /// <inheritdoc />
    public virtual Task<RequirementDecision> MustAsync(IPipelineHookContext context)
        => Task.FromResult(Must(context));

    /// <summary>
    /// Synchronous version of requirement evaluation for simpler implementations.
    /// </summary>
    /// <param name="context">The pipeline context for evaluation.</param>
    /// <returns>The requirement decision.</returns>
    /// <remarks>
    /// Override this method for synchronous requirements instead of <see cref="MustAsync"/>.
    /// The default implementation returns <see cref="Pass"/>.
    /// </remarks>
    protected virtual RequirementDecision Must(IPipelineHookContext context)
        => Pass();

    /// <summary>
    /// Creates a passed requirement decision.
    /// </summary>
    /// <returns>A <see cref="RequirementDecision"/> indicating the requirement is satisfied.</returns>
    protected static RequirementDecision Pass() => RequirementDecision.Passed;

    /// <summary>
    /// Creates a failed requirement decision with a reason.
    /// </summary>
    /// <param name="reason">The reason the requirement failed.</param>
    /// <returns>A <see cref="RequirementDecision"/> indicating the requirement is not satisfied.</returns>
    protected static RequirementDecision Fail(string reason) => RequirementDecision.Failed(reason);

    /// <summary>
    /// Creates a requirement decision based on a boolean condition.
    /// </summary>
    /// <param name="condition">True if the requirement is satisfied.</param>
    /// <param name="failureReason">The reason to provide if the condition is false.</param>
    /// <returns>A <see cref="RequirementDecision"/> based on the condition.</returns>
    protected static RequirementDecision When(bool condition, string failureReason)
        => RequirementDecision.Of(condition, failureReason);
}
