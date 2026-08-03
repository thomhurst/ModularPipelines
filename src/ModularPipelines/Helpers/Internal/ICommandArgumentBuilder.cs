namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Service that builds a string argument list from a command model and an options object instance.
/// </summary>
public interface ICommandArgumentBuilder
{
    /// <summary>
    /// Builds the list of arguments based on the values in an options object.
    /// </summary>
    /// <param name="commandModel">The structured command model for the options type.</param>
    /// <param name="optionsObject">The options object instance containing the values.</param>
    /// <returns>A list of string arguments ready to be passed to a CLI tool.</returns>
    IReadOnlyList<string> BuildArguments(IReadOnlyList<PropertyCommandLinePart> commandModel, object optionsObject);

    /// <summary>
    /// Builds the arguments and reports whether the renderer emitted an option terminator.
    /// </summary>
    /// <param name="commandModel">The structured command model for the options type.</param>
    /// <param name="optionsObject">The options object instance containing the values.</param>
    /// <param name="emittedOptionTerminator">
    /// Whether an earlier render emitted <c>--</c>; updated when this render emits it.
    /// </param>
    /// <returns>A list of string arguments ready to be passed to a CLI tool.</returns>
    IReadOnlyList<string> BuildArguments(
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        object optionsObject,
        ref bool emittedOptionTerminator);
}
