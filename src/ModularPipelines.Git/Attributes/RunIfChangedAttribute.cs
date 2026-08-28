using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

/// <summary>
/// Runs a module when a path changed relative to a Git merge base.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
[ExcludeFromCodeCoverage]
public sealed class RunIfChangedAttribute : Attribute, IGroupedConditionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RunIfChangedAttribute"/> class.
    /// </summary>
    /// <param name="pathPatterns">Repository-relative glob patterns. A match against any pattern runs the module.</param>
    public RunIfChangedAttribute(params string[] pathPatterns)
    {
        ArgumentNullException.ThrowIfNull(pathPatterns);
        if (pathPatterns.Length == 0)
        {
            throw new ArgumentException("At least one path pattern is required.", nameof(pathPatterns));
        }

        foreach (var pathPattern in pathPatterns)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(pathPattern);
        }

        PathPatterns = Array.AsReadOnly(pathPatterns.ToArray());
    }

    /// <summary>
    /// Gets the repository-relative glob patterns.
    /// </summary>
    public IReadOnlyList<string> PathPatterns { get; }

    /// <summary>
    /// Gets or sets the revision whose merge base with HEAD starts the comparison.
    /// </summary>
    public string Base { get; set; } = "origin/main";

    public ConditionLogic Logic => ConditionLogic.Any;

    public Type ConditionGroupType => typeof(RunIfChangedAttribute);

    public string ConditionNames =>
        $"{nameof(RunIfChangedAttribute)}({string.Join(", ", PathPatterns)}; Base={Base})";

    public Task<bool> EvaluateAsync(IPipelineContext context) =>
        EvaluateAsync(context, default);

    public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        context.Tools.Git.Changes.HasChangesAsync(PathPatterns, Base, cancellationToken);
}
