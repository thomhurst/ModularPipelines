using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal record ModuleDependencyModel(IModule Module)
{
    public List<ModuleDependencyModel> IsDependencyFor { get; } = [];

    public List<ModuleDependencyModel> IsDependentOn { get; } = [];

    public IEnumerable<ModuleDependencyModel> AllDescendantDependenciesAndSelf()
    {
        yield return this;

        foreach (var dependency in AllDescendantDependencies())
        {
            yield return dependency;
        }
    }

    public IEnumerable<ModuleDependencyModel> AllDescendantDependencies()
    {
        var remainingDependencies = new Queue<ModuleDependencyModel>(IsDependentOn);
        var visitedDependencies = new HashSet<ModuleDependencyModel>();

        while (remainingDependencies.TryDequeue(out var dependency))
        {
            if (!visitedDependencies.Add(dependency))
            {
                continue;
            }

            yield return dependency;

            foreach (var descendantDependency in dependency.IsDependentOn)
            {
                remainingDependencies.Enqueue(descendantDependency);
            }
        }
    }

    public virtual bool Equals(ModuleDependencyModel? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Module.GetType() == other.Module.GetType();
    }

    public override int GetHashCode()
    {
        return Module.GetType().GetHashCode();
    }
}
