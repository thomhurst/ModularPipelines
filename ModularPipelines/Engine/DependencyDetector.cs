using ModularPipelines.Helpers;

namespace ModularPipelines.Engine;

internal class DependencyDetector : IDependencyDetector
{
    private readonly IDependencyCollisionDetector _dependencyCollisionDetector;
    private readonly IDependencyPrinter _dependencyPrinter;

    public DependencyDetector(IDependencyCollisionDetector dependencyCollisionDetector,
        IDependencyPrinter dependencyPrinter)
    {
        _dependencyCollisionDetector = dependencyCollisionDetector;
        _dependencyPrinter = dependencyPrinter;
    }

    public void Check()
    {
        _dependencyCollisionDetector.CheckCollisions();
        _dependencyPrinter.Print();
    }
}