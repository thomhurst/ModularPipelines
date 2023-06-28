namespace ModularPipelines.Helpers;

public interface IDependencyCollisionDetector
{
    void CheckDependencies();
    void CheckDependency(Type dependentType, Type dependencyType);
}