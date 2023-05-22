using System.Collections.Concurrent;
using Pipeline.NET.Exceptions;

namespace Pipeline.NET;

internal class DependencyCollisionDetector : IDependencyCollisionDetector
{
    public readonly ConcurrentDictionary<Type, ConcurrentBag<Type>> _history = new();

    public void CheckDependency(Type dependentType, Type dependencyType)
    {
        var existingDependenciesOfDependencyToAdd = _history.GetOrAdd(dependencyType, new ConcurrentBag<Type>());

        if (existingDependenciesOfDependencyToAdd.Contains(dependentType))
        {
            throw new DependencyCollisionException($"{dependentType.FullName} and {dependencyType.FullName} are dependent on each other");
        }
        
        var existingDependencies = _history.GetOrAdd(dependentType, new ConcurrentBag<Type>());
        
        existingDependencies.Add(dependencyType);
    }
}