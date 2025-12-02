using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

internal class DependencyCollisionDetector : IDependencyCollisionDetector
{
    private readonly IServiceProvider _serviceProvider;
    private IDependencyChainProvider? _dependencyChainProvider;

    public DependencyCollisionDetector(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private IDependencyChainProvider DependencyChainProvider =>
        _dependencyChainProvider ??= _serviceProvider.GetRequiredService<IDependencyChainProvider>();

    public void CheckCollisions()
    {
        foreach (var moduleDependencyModel in DependencyChainProvider.ModuleDependencyModels)
        {
            CheckCollision(moduleDependencyModel);
        }
    }

    private static void CheckCollision(ModuleDependencyModel moduleDependencyModel)
    {
        // Check for direct self-reference first
        var moduleType = moduleDependencyModel.Module.GetType();
        if (moduleDependencyModel.IsDependentOn.Any(d => d.Module.GetType() == moduleType))
        {
            throw new ModuleReferencingSelfException(
                $"Module '{moduleType.Name}' cannot reference itself. " +
                "A module cannot depend on its own result.");
        }

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