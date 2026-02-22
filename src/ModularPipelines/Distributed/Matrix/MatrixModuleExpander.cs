using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Matrix;

/// <summary>
/// Scans registered modules for [MatrixTarget] attributes and expands them into
/// N synthetic module registrations, one per target value.
/// </summary>
internal class MatrixModuleExpander(ILogger<MatrixModuleExpander> logger)
{
    private readonly ILogger<MatrixModuleExpander> _logger = logger;
    private readonly Dictionary<Type, List<MatrixModuleInstance>> _expansions = [];

    /// <summary>
    /// Gets the expanded instances for a module type, if any.
    /// </summary>
    public IReadOnlyList<MatrixModuleInstance>? GetExpansions(Type moduleType)
    {
        return _expansions.TryGetValue(moduleType, out var list) ? list : null;
    }

    /// <summary>
    /// Scans module types for MatrixTarget attributes and records expansion metadata.
    /// Actual module registration expansion is handled by the plugin during pipeline initialization.
    /// </summary>
    public void ScanForExpansions(IEnumerable<IModule> modules)
    {
        foreach (var module in modules)
        {
            var moduleType = module.GetType();
            var matrixAttr = moduleType.GetCustomAttributes(typeof(MatrixTargetAttribute), true)
                .Cast<MatrixTargetAttribute>()
                .FirstOrDefault();

            if (matrixAttr is null || matrixAttr.Targets.Length == 0)
            {
                continue;
            }

            var instances = new List<MatrixModuleInstance>();
            foreach (var target in matrixAttr.Targets)
            {
                var instance = new MatrixModuleInstance(
                    OriginalType: moduleType,
                    TargetValue: target,
                    InstanceName: $"{moduleType.Name}[{target}]",
                    CapabilityName: target
                );
                instances.Add(instance);
            }

            _expansions[moduleType] = instances;
            _logger.LogInformation(
                "Matrix module {Module} expanded into {Count} instances: {Targets}",
                moduleType.Name, instances.Count, string.Join(", ", matrixAttr.Targets));
        }
    }
}
