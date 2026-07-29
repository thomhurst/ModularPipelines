using System.Collections.Concurrent;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

internal sealed class ModuleLookup
{
    private readonly ConcurrentDictionary<Type, IReadOnlyList<IModule>> _assignableModules = new();
    private readonly IReadOnlyDictionary<Type, IReadOnlyList<IModule>> _ambiguousConcreteModules;
    private readonly IReadOnlyDictionary<Type, IModule> _concreteModules;
    private readonly IReadOnlyList<IModule> _modules;

    public ModuleLookup(IEnumerable<IModule> registeredModules)
    {
        _modules =
        [
            .. registeredModules.Distinct<IModule>(ReferenceEqualityComparer.Instance),
        ];

        var modulesByConcreteType = _modules
            .GroupBy(x => x.GetType())
            .ToDictionary(
                x => x.Key,
                x => (IReadOnlyList<IModule>) [.. x]);

        _concreteModules = modulesByConcreteType
            .Where(x => x.Value.Count == 1)
            .ToDictionary(x => x.Key, x => x.Value[0]);
        _ambiguousConcreteModules = modulesByConcreteType
            .Where(x => x.Value.Count > 1)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public IModule? GetAssignable(Type type)
    {
        if (type.IsSealed)
        {
            return GetExact(type);
        }

        var matches = _assignableModules.GetOrAdd(
            type,
            requestedType => [.. _modules.Where(requestedType.IsInstanceOfType)]);

        return GetSingleMatch(type, matches);
    }

    public IModule? GetExact(Type type)
    {
        ThrowIfAmbiguous(type, _ambiguousConcreteModules);
        return _concreteModules.GetValueOrDefault(type);
    }

    private static IModule? GetSingleMatch(Type requestedType, IReadOnlyList<IModule> matches)
    {
        if (matches.Count > 1)
        {
            throw new AmbiguousModuleException(
                requestedType,
                [.. matches.Select(x => x.GetType())]);
        }

        return matches.Count == 1 ? matches[0] : null;
    }

    private static void ThrowIfAmbiguous(
        Type requestedType,
        IReadOnlyDictionary<Type, IReadOnlyList<IModule>> ambiguousModules)
    {
        if (ambiguousModules.TryGetValue(requestedType, out var matches))
        {
            throw new AmbiguousModuleException(
                requestedType,
                [.. matches.Select(x => x.GetType())]);
        }
    }
}
