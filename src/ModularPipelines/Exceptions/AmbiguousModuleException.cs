namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a module lookup matches more than one registered module.
/// </summary>
/// <remarks>
/// Request a concrete module type when multiple registered modules inherit from or implement
/// the requested type.
/// </remarks>
/// <seealso cref="PipelineException"/>
public class AmbiguousModuleException : PipelineException
{
    /// <summary>
    /// Initialises a new instance of the <see cref="AmbiguousModuleException"/> class.
    /// </summary>
    /// <param name="requestedType">The module type requested by the lookup.</param>
    /// <param name="matchingModuleTypes">The registered module types matched by the lookup.</param>
    public AmbiguousModuleException(Type requestedType, IReadOnlyList<Type> matchingModuleTypes)
        : base(CreateMessage(requestedType, matchingModuleTypes))
    {
        RequestedType = requestedType;
        MatchingModuleTypes = matchingModuleTypes;
    }

    /// <summary>
    /// Gets the module type requested by the lookup.
    /// </summary>
    public Type RequestedType { get; }

    /// <summary>
    /// Gets the registered module types matched by the lookup.
    /// </summary>
    public IReadOnlyList<Type> MatchingModuleTypes { get; }

    private static string CreateMessage(Type requestedType, IReadOnlyList<Type> matchingModuleTypes)
    {
        var matches = string.Join(
            ", ",
            matchingModuleTypes.Select(x => $"'{x.FullName ?? x.Name}'"));

        return $"Module lookup for '{requestedType.FullName ?? requestedType.Name}' matched multiple registered modules: " +
               $"{matches}. Request a concrete module type.";
    }
}
