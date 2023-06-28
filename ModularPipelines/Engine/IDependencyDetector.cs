namespace ModularPipelines;

public interface IDependencyDetector
{
    IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; }
    void Print();
}