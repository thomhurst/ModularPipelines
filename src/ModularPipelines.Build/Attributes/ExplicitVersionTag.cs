namespace ModularPipelines.Build.Attributes;

public sealed class ExplicitVersionTag : IRunCondition, IPlanningSafe
{
    public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // A category marks membership; require the dedicated execution path to opt into tagging.
        var categories = context.Services.Options.RunOnlyCategories;
        return Task.FromResult(categories is [var category]
            && string.Equals(category, "VersionTag", StringComparison.OrdinalIgnoreCase));
    }
}
