namespace ModularPipelines.Helpers;

public interface IDependencyCollisionDetector
{
    void CheckDependency(Type dependentType, Type dependencyType);
}