using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
using Semaphores;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies a parallel execution limit for a module using a strongly-typed limit class.
/// </summary>
/// <typeparam name="TParallelLimit">The type implementing <see cref="IParallelLimit"/>.</typeparam>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ParallelLimiterAttribute<TParallelLimit> : ParallelLimiterAttribute
    where TParallelLimit : IParallelLimit
{
    public ParallelLimiterAttribute() : base(typeof(TParallelLimit))
    {
    }

    /// <inheritdoc />
    internal override AsyncSemaphore GetLock(IParallelLimitProvider provider)
    {
        return provider.GetLock<TParallelLimit>();
    }
}

/// <summary>
/// Base attribute for specifying parallel execution limits.
/// </summary>
public abstract class ParallelLimiterAttribute : Attribute
{
    /// <summary>
    /// Gets the type implementing <see cref="IParallelLimit"/>.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParallelLimiterAttribute"/> class.
    /// </summary>
    /// <param name="type">The type implementing <see cref="IParallelLimit"/>.</param>
    protected ParallelLimiterAttribute(Type type)
    {
        if (!type.IsAssignableTo(typeof(IParallelLimit)))
        {
            throw new ArgumentException("Type must implement IParallelLimit", nameof(type));
        }

        Type = type;
    }

    /// <summary>
    /// Gets the semaphore lock from the provider without reflection.
    /// </summary>
    /// <param name="provider">The parallel limit provider.</param>
    /// <returns>The semaphore for this limit type.</returns>
    internal abstract AsyncSemaphore GetLock(IParallelLimitProvider provider);
}