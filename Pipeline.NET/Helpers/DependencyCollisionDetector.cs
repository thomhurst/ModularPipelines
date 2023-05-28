using System.Collections.Concurrent;
using Pipeline.NET.Exceptions;

namespace Pipeline.NET.Helpers;

internal class DependencyCollisionDetector : IDependencyCollisionDetector
{
    private readonly ConcurrentDictionary<Type, ConcurrentBag<Type>> _history = new();

    public void CheckDependency(Type dependentType, Type dependencyType)
    {
        CheckDependency(dependentType, dependencyType, new()
        {
            $"**{dependentType.FullName}**"
        }, true);
    }

    private void CheckDependency(Type dependentType, Type dependencyType, List<string> enumeratedTypes, bool shouldAdd)
    {
        enumeratedTypes.Add(dependencyType.FullName!);
        
        var existingDependenciesOfDependencyToAdd = _history.GetOrAdd(dependencyType, new ConcurrentBag<Type>());

        if (existingDependenciesOfDependencyToAdd.Contains(dependentType))
        {
            enumeratedTypes.Add($"**{dependentType.FullName}**");
            var typeChain = string.Join(" -> ", enumeratedTypes);
            throw new DependencyCollisionException($"Dependency collision detected: {typeChain}");
        }
        
        foreach (var innerDependencyType in existingDependenciesOfDependencyToAdd)
        {
            CheckDependency(dependentType, innerDependencyType, enumeratedTypes.ToList(), false);
        }

        if (shouldAdd)
        {
            var existingDependencies = _history.GetOrAdd(dependentType, new ConcurrentBag<Type>());

            existingDependencies.Add(dependencyType);
        }
    }
}