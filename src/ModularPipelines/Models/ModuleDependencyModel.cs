using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal record ModuleDependencyModel(IModule Module)
{
    public List<ModuleDependencyModel> IsDependencyFor { get; } = new();

    public List<ModuleDependencyModel> IsDependentOn { get; } = new();

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
        foreach (var moduleDependencyModel in IsDependentOn)
        {
            yield return moduleDependencyModel;

            if (moduleDependencyModel.Module.GetType() == Module.GetType())
            {
                yield break;
            }
        }

        foreach (var moduleDependencyModel in IsDependentOn.SelectMany(d => d.AllDescendantDependencies()))
        {
            yield return moduleDependencyModel;

            if (moduleDependencyModel.Module.GetType() == Module.GetType())
            {
                yield break;
            }
        }
    }

    public virtual bool Equals(ModuleDependencyModel? other)
    {
        if (ReferenceEquals(null, other))
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
