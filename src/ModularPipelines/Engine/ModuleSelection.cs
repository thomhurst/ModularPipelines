using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal sealed class ModuleSelection
{
    private readonly IReadOnlySet<Type>? _targetTypes;
    private readonly IReadOnlySet<Type> _skippedTypes;

    private ModuleSelection(IReadOnlySet<Type>? targetTypes, IReadOnlySet<Type> skippedTypes)
    {
        _targetTypes = targetTypes;
        _skippedTypes = skippedTypes;
    }

    public static ModuleSelection Create(
        IReadOnlyList<IModule> modules,
        IDependencyChainProvider dependencyChainProvider,
        PipelineOptions options)
    {
        if (options.TargetModules is not { Count: > 0 }
            && options.SkippedModules is not { Count: > 0 })
        {
            return new ModuleSelection(null, new HashSet<Type>());
        }

        dependencyChainProvider.Initialize(modules);
        var models = dependencyChainProvider.ModuleDependencyModels;
        var skippedTypes = ResolveNames(options.SkippedModules, models, "skip");
        var targetModels = ResolveModels(options.TargetModules, models, "target");
        IReadOnlySet<Type>? targetTypes = targetModels.Count == 0
            ? null
            : targetModels
                .SelectMany(model => model.AllDescendantDependenciesAndSelf())
                .Select(model => model.Module.GetType())
                .ToHashSet();

        return new ModuleSelection(targetTypes, skippedTypes);
    }

    public SkipDecision? GetSkipDecision(IModule module)
    {
        var moduleType = module.GetType();
        if (_skippedTypes.Contains(moduleType))
        {
            return SkipDecision.Skip($"Module '{moduleType.Name}' was skipped by pipeline selection");
        }

        return _targetTypes is not null && !_targetTypes.Contains(moduleType)
            ? SkipDecision.Skip($"Module '{moduleType.Name}' was not in the targeted dependency closure")
            : null;
    }

    private static IReadOnlySet<Type> ResolveNames(
        IReadOnlyList<string>? names,
        IReadOnlyList<ModuleDependencyModel> models,
        string selectionKind) =>
        ResolveModels(names, models, selectionKind)
            .Select(model => model.Module.GetType())
            .ToHashSet();

    private static IReadOnlyList<ModuleDependencyModel> ResolveModels(
        IReadOnlyList<string>? names,
        IReadOnlyList<ModuleDependencyModel> models,
        string selectionKind)
    {
        if (names is null || names.Count == 0)
        {
            return [];
        }

        var resolved = new List<ModuleDependencyModel>();
        foreach (var name in names)
        {
            var matches = models
                .Where(model => Matches(model.Module.GetType(), name))
                .ToArray();
            if (matches.Length == 0)
            {
                throw new ModuleSelectionException(
                    $"Pipeline {selectionKind} module '{name}' is not registered.");
            }

            if (matches.Length > 1)
            {
                throw new ModuleSelectionException(
                    $"Pipeline {selectionKind} module name '{name}' is ambiguous. "
                    + $"Use a full type name: {string.Join(", ", matches.Select(model => model.Module.GetType().FullName))}.");
            }

            resolved.Add(matches[0]);
        }

        return resolved.Distinct().ToArray();
    }

    private static bool Matches(Type moduleType, string name) =>
        name.Equals(moduleType.Name, StringComparison.OrdinalIgnoreCase)
        || name.Equals(moduleType.FullName, StringComparison.OrdinalIgnoreCase)
        || name.Equals(moduleType.AssemblyQualifiedName, StringComparison.OrdinalIgnoreCase);
}
