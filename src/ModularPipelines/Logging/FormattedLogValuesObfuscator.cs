using ModularPipelines.Engine;

namespace ModularPipelines.Logging;

/// <summary>
/// Handles obfuscation of values within structured logging state.
/// </summary>
/// <remarks>
/// Structured values keep their original type when their string representation contains no
/// secrets. Values containing secrets are replaced with their obfuscated string representation.
/// </remarks>
/// <example>
/// <code>
/// // Typically injected and used internally by ModuleLogger
/// var obfuscator = new FormattedLogValuesObfuscator(secretObfuscator);
///
/// // Called during logging to mask secrets in structured log values
/// logger.LogInformation("Connection string: {ConnectionString}", sensitiveValue);
/// // Before obfuscation: "Connection string: Server=prod;Password=secret123"
/// // After obfuscation: "Connection string: Server=prod;Password=**********"
/// </code>
/// </example>
internal class FormattedLogValuesObfuscator : IFormattedLogValuesObfuscator
{
    private readonly ISecretObfuscator _secretObfuscator;

    public FormattedLogValuesObfuscator(ISecretObfuscator secretObfuscator)
    {
        _secretObfuscator = secretObfuscator;
    }

    public object TryObfuscateValues(object state)
    {
        var hasSecrets = _secretObfuscator.HasSecrets;

        if (state is not IReadOnlyList<KeyValuePair<string, object?>> values)
        {
            return hasSecrets ? ObfuscateValue(state) : state;
        }

        KeyValuePair<string, object?>[]? obfuscatedValues = null;

        for (var index = 0; index < values.Count; index++)
        {
            var property = values[index];
            if (property.Value is null)
            {
                continue;
            }

            object obfuscatedValue;
            if (property.Value is PreObfuscatedLogValue preObfuscatedValue)
            {
                obfuscatedValue = preObfuscatedValue.Value;
            }
            else if (hasSecrets)
            {
                obfuscatedValue = ObfuscateValue(property.Value);
            }
            else
            {
                continue;
            }

            if (ReferenceEquals(obfuscatedValue, property.Value))
            {
                continue;
            }

            obfuscatedValues ??= values.ToArray();
            obfuscatedValues[index] = new KeyValuePair<string, object?>(property.Key, obfuscatedValue);
        }

        return obfuscatedValues ?? state;
    }

    private object ObfuscateValue(object value)
    {
        if (value is PreObfuscatedLogValue preObfuscatedValue)
        {
            return preObfuscatedValue.Value;
        }

        string originalValue;
        try
        {
            originalValue = value as string ?? value.ToString() ?? string.Empty;
        }
        catch (Exception)
        {
            return value;
        }

        var obfuscatedValue = _secretObfuscator.Obfuscate(originalValue, null);
        return ReferenceEquals(obfuscatedValue, originalValue)
               || obfuscatedValue.Equals(originalValue, StringComparison.Ordinal)
            ? value
            : obfuscatedValue;
    }
}

/// <summary>
/// Marks a structured log value that has already passed through secret obfuscation.
/// </summary>
internal sealed record PreObfuscatedLogValue(string Value)
{
    public override string ToString() => Value;
}

/// <summary>
/// Interface for obfuscating formatted log values.
/// </summary>
internal interface IFormattedLogValuesObfuscator
{
    /// <summary>
    /// Attempts to obfuscate values within structured logging state.
    /// </summary>
    /// <param name="state">The state object to obfuscate.</param>
    /// <returns>The original state when unchanged; otherwise, sanitized structured state.</returns>
    object TryObfuscateValues(object state);
}
