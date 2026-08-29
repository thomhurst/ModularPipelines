using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

/// <summary>
/// A requirement that uses a delegate for evaluation, enabling inline requirement definitions.
/// </summary>
/// <remarks>
/// <para>
/// This class allows creating requirements without defining a new class,
/// useful for simple or one-off requirements.
/// </para>
/// <para><b>Example with async delegate:</b></para>
/// <code>
/// services.AddRequirement(Require.ThatAsync(
///     async (context, cancellationToken) =&gt; {
///         var result = await context.Shell.RunAsync(
///             "docker",
///             ["--version"],
///             cancellationToken: cancellationToken);
///         return result.ExitCode == 0;
///     },
///     "Docker must be installed"));
/// </code>
/// <para><b>Example with sync delegate:</b></para>
/// <code>
/// services.AddRequirement(Require.That(
///     context =&gt; Environment.Is64BitProcess,
///     "64-bit process is required"));
/// </code>
/// </remarks>
/// <seealso cref="Require"/>
public sealed class DelegateRequirement : IPipelineRequirement
{
    private readonly Func<IPipelineContext, CancellationToken, Task<bool>> _evaluator;
    private readonly string _failureReason;
    private readonly int _order;

    /// <summary>
    /// Initialises a new instance of the <see cref="DelegateRequirement"/> class with an async evaluator.
    /// </summary>
    /// <param name="evaluator">The async delegate that evaluates the requirement.</param>
    /// <param name="failureReason">The reason to display if the requirement fails.</param>
    /// <param name="order">The evaluation order. Lower values are evaluated first.</param>
    internal DelegateRequirement(
        Func<IPipelineContext, CancellationToken, Task<bool>> evaluator,
        string failureReason,
        int order = 0)
    {
        _evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
        _failureReason = failureReason ?? throw new ArgumentNullException(nameof(failureReason));
        _order = order;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DelegateRequirement"/> class with a sync evaluator.
    /// </summary>
    /// <param name="evaluator">The delegate that evaluates the requirement.</param>
    /// <param name="failureReason">The reason to display if the requirement fails.</param>
    /// <param name="order">The evaluation order. Lower values are evaluated first.</param>
    internal DelegateRequirement(
        Func<IPipelineContext, bool> evaluator,
        string failureReason,
        int order = 0)
        : this((ctx, _) => Task.FromResult(evaluator(ctx)), failureReason, order)
    {
        ArgumentNullException.ThrowIfNull(evaluator);
    }

    /// <inheritdoc />
    public int Order => _order;

    /// <inheritdoc />
    public async Task<RequirementDecision> EvaluateAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var passed = await _evaluator(context, cancellationToken).ConfigureAwait(false);
        return passed ? RequirementDecision.Passed : RequirementDecision.Failed(_failureReason);
    }
}
