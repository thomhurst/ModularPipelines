namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Service that handles placeholder replacement in command parts.
/// Placeholders (e.g., &lt;argument&gt; or [&lt;optional&gt;]) in command templates
/// are replaced with actual values from the options object.
/// </summary>
public interface IPlaceholderHandler
{
    /// <summary>
    /// Replaces placeholder strings in command parts with actual argument values.
    /// Placeholders are matched to properties via CliArgumentAttribute.Name.
    /// </summary>
    /// <param name="commandParts">The raw command parts that may contain placeholders.</param>
    /// <param name="optionsObject">The options object containing values for the placeholders.</param>
    /// <returns>A list of command parts with placeholders replaced by their values.</returns>
    List<string> ReplacePlaceholders(List<string> commandParts, object optionsObject);
}
