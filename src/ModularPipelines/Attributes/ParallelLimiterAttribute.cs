using ModularPipelines.Interfaces;

namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ParallelLimiterAttribute<TParallelLimit> : ParallelLimiterAttribute
    where TParallelLimit : IParallelLimit, new()
{
    public ParallelLimiterAttribute() : base(typeof(TParallelLimit))
    {
    }
}

public class ParallelLimiterAttribute : Attribute
{
    public Type Type { get; }

    public ParallelLimiterAttribute(Type type)
    {
        if (!type.IsAssignableTo(typeof(IParallelLimit)))
        {
            throw new Exception("Type must be of IParallelLimit");
        }
        
        Type = type;
    }
}