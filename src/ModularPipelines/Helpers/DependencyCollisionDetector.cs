using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

internal class DependencyCollisionDetector : IDependencyCollisionDetector
{
    private readonly IDependencyChainProvider _dependencyChainProvider;

    public DependencyCollisionDetector(IDependencyChainProvider dependencyChainProvider)
    {
        _dependencyChainProvider = dependencyChainProvider;
    }

    public void CheckCollisions()
    {
        foreach (var moduleDependencyModel in _dependencyChainProvider.ModuleDependencyModels)
        {
            CheckCollision(moduleDependencyModel);
        }
    }

    private static void CheckCollision(ModuleDependencyModel moduleDependencyModel)
    {
        var allDescendentDependenciesAndSelf = moduleDependencyModel.AllDescendantDependenciesAndSelf().ToList();
        var allDescendentDependencies = allDescendentDependenciesAndSelf.Skip(1).ToList();

        if (!allDescendentDependencies.Contains(moduleDependencyModel))
        {
            return;
        }

        var index = allDescendentDependencies.IndexOf(moduleDependencyModel) + 1;

        var formattedArray = allDescendentDependenciesAndSelf
            .Take(index + 1)
            .Select(x => x.Module.GetType().Name)
            .ToArray();

        formattedArray[0] = $"**{formattedArray[0]}**";
        formattedArray[^1] = $"**{formattedArray[^1]}**";

        var typeChain = string.Join(" -> ", formattedArray);
        throw new DependencyCollisionException($"Dependency collision detected: {typeChain}");
    }
}
