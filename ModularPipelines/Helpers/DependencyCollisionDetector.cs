using System.Collections.Concurrent;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Helpers;

internal class DependencyCollisionDetector : IDependencyCollisionDetector
{
    private readonly IDependencyDetector _dependencyDetector;
    private readonly ConcurrentDictionary<Type, ConcurrentBag<Type>> _history = new();

    public DependencyCollisionDetector(IDependencyDetector dependencyDetector)
    {
        _dependencyDetector = dependencyDetector;
    }
    
    public void CheckDependency(Type dependentType, Type dependencyType)
    {
        CheckDependency(dependentType, dependencyType, new()
        {
            $"**{dependentType.FullName}**"
        }, true);
    }
    
    public void CheckDependencies()
    {
        foreach (var moduleDependencyModel in _dependencyDetector.ModuleDependencyModels)
        {
            var allDescendentDependencies = GetDescendents(moduleDependencyModel).ToList();

            var backwardsDependencyReference = allDescendentDependencies.FirstOrDefault(x => x.IsDependentOn.Contains(moduleDependencyModel));

            if (backwardsDependencyReference != null)
            {
                var index = allDescendentDependencies.IndexOf(backwardsDependencyReference);
                var typeChain = string.Join(" -> ", allDescendentDependencies.Take(index + 1));
                throw new DependencyCollisionException($"Dependency collision detected: {typeChain}");
            }
        }
    }

    private IEnumerable<ModuleDependencyModel> GetDescendents(ModuleDependencyModel moduleDependencyModel)
    {
        foreach (var directDependency in moduleDependencyModel.IsDependentOn)
        {
            yield return directDependency;

            foreach (var nestedDependency in directDependency.IsDependentOn.SelectMany(GetDescendents))
            {
                yield return nestedDependency;
            }
        }
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