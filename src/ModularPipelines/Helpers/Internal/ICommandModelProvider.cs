namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Internal service for creating a command model from an options object's type.
/// </summary>
internal interface ICommandModelProvider
{
    /// <summary>
    /// Gets a cached, structured command model by reflecting over the attributes of an options type.
    /// </summary>
    /// <param name="optionsType">The Type of the options class.</param>
    /// <returns>A readonly list of parts representing the command structure.</returns>
    IReadOnlyList<PropertyCommandLinePart> GetCommandModel(Type optionsType);
}
