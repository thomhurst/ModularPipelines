namespace ModularPipelines.Models;

/// <summary>
/// Represents two consecutive values belonging to one command-line option.
/// </summary>
/// <param name="First">The first option value.</param>
/// <param name="Second">The second option value.</param>
/// <example><c>--arg name value</c></example>
public record CliOptionValuePair(string First, string Second);
