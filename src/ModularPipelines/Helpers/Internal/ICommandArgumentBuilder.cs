namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Internal service that builds a string argument list from a command model and an options object instance.
/// </summary>
internal interface ICommandArgumentBuilder
{
    /// <summary>
    /// Builds the list of arguments based on the values in an options object.
    /// </summary>
    /// <param name="commandModel">The structured command model for the options type.</param>
    /// <param name="optionsObject">The options object instance containing the values.</param>
    /// <returns>A list of string arguments ready to be passed to a CLI tool.</returns>
    List<string> BuildArguments(IReadOnlyList<PropertyCommandLinePart> commandModel, object optionsObject);
}
