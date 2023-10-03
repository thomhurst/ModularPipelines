using ModularPipelines.Helpers;

namespace ModularPipelines.Engine;

internal class DependencyDetector : IDependencyDetector
{
    private readonly IUnusedModuleDetector _unusedModuleDetector;
    private readonly IDependencyCollisionDetector _dependencyCollisionDetector;
    private readonly IDependencyPrinter _dependencyPrinter;

    public DependencyDetector(IUnusedModuleDetector unusedModuleDetector,
        IDependencyCollisionDetector dependencyCollisionDetector,
        IDependencyPrinter dependencyPrinter)
    {
        _unusedModuleDetector = unusedModuleDetector;
        _dependencyCollisionDetector = dependencyCollisionDetector;
        _dependencyPrinter = dependencyPrinter;
    }

    public void Check()
    {
        _unusedModuleDetector.Log();
        _dependencyCollisionDetector.CheckCollisions();
        _dependencyPrinter.PrintDependencyChains();
    }
}
