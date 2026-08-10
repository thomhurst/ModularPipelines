namespace ModularPipelines.Models;

/// <summary>
/// Represents two consecutive values belonging to one command-line option.
/// Null operands are rejected during argument construction. Non-null operands are rendered
/// literally, including empty and whitespace values.
/// </summary>
/// <param name="First">The first option value.</param>
/// <param name="Second">The second option value.</param>
/// <example><c>--arg name value</c>.</example>
public record CliValuePair(string? First, string? Second);
