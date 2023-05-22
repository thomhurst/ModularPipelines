namespace Pipeline.NET;

public interface IDependencyCollisionDetector
{
    void CheckDependency(Type dependentType, Type dependencyType);
}