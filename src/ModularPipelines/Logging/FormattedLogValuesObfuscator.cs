using System.Collections;
using ModularPipelines.Secrets;

namespace ModularPipelines.Logging;

/// <summary>
/// Handles obfuscation of values within structured logging state.
/// </summary>
/// <remarks>
/// When masking is active, scalar values retain their types unless they contain secrets.
/// Collections are copied with sanitized elements, and complex objects become masked text
/// so providers cannot destructure the original objects and expose hidden secrets.
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
internal class FormattedLogValuesObfuscator(ISecretObfuscator secretObfuscator) : IFormattedLogValuesObfuscator
{
    private readonly ISecretObfuscator _secretObfuscator = secretObfuscator;

    public object TryObfuscateValues(object state)
    {
        // HasSecrets is only a hint; custom obfuscators may still apply policy-based masking.
        if (_secretObfuscator is not SecretObfuscator builtInObfuscator)
        {
            return TryObfuscateValues(state, skipOrdinaryValues: false);
        }

        var registrationState = builtInObfuscator.GetRegistrationState();
        var skipOrdinaryValues = !registrationState.HasSecrets;
        var obfuscatedState = TryObfuscateValues(state, skipOrdinaryValues);

        if (!skipOrdinaryValues)
        {
            return obfuscatedState;
        }

        // A secret may be registered while the zero-secret fast path traverses the state.
        var currentRegistrationState = builtInObfuscator.GetRegistrationState();
        return currentRegistrationState.Version != registrationState.Version
               && currentRegistrationState.HasSecrets
            ? TryObfuscateValues(state, skipOrdinaryValues: false)
            : obfuscatedState;
    }

    private object TryObfuscateValues(object state, bool skipOrdinaryValues)
    {
        if (state is not IReadOnlyList<KeyValuePair<string, object?>> values)
        {
            return skipOrdinaryValues ? state : ObfuscateValue(state);
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
            else if (!skipOrdinaryValues)
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

            obfuscatedValues ??= [.. values];
            obfuscatedValues[index] = new KeyValuePair<string, object?>(property.Key, obfuscatedValue);
        }

        return obfuscatedValues ?? state;
    }

    private object ObfuscateValue(object value)
    {
        // Bound the whole traversal, including nested and cyclic collections.
        var remainingValues = 1024;
        return ObfuscateValue(value, depth: 0, ref remainingValues);
    }

    private object ObfuscateValue(object value, int depth, ref int remainingValues)
    {
        if (depth >= 16 || remainingValues-- <= 0)
        {
            return _secretObfuscator.Obfuscate("[Truncated log value]", null);
        }

        if (value is PreObfuscatedLogValue preObfuscatedValue)
        {
            return preObfuscatedValue.Value;
        }

        if (value is IEnumerable enumerable && value is not string)
        {
            return ObfuscateEnumerable(enumerable, depth, ref remainingValues);
        }

        string originalValue;
        try
        {
            originalValue = value as string ?? value.ToString() ?? string.Empty;
        }
        catch (Exception)
        {
            // Never let a provider inspect an object that could not be sanitized.
            return _secretObfuscator.Obfuscate("[Unavailable log value]", null);
        }

        var obfuscatedValue = _secretObfuscator.Obfuscate(originalValue, null);
        var type = value.GetType();
        var isScalar = type.IsPrimitive || type.IsEnum
                       || value is string or decimal or DateTime or DateTimeOffset
                           or DateOnly or TimeOnly or TimeSpan or Guid;

        // A harmless ToString() does not prove an object's properties are safe to export.
        return isScalar && obfuscatedValue.Equals(originalValue, StringComparison.Ordinal)
            ? value
            : obfuscatedValue;
    }

    private object ObfuscateEnumerable(IEnumerable enumerable, int depth, ref int remainingValues)
    {
        var values = new List<object?>();
        try
        {
            foreach (var element in enumerable)
            {
                if (remainingValues <= 0)
                {
                    values.Add(_secretObfuscator.Obfuscate("[Truncated log value]", null));
                    break;
                }

                if (element is null)
                {
                    remainingValues--;
                    values.Add(null);
                }
                else
                {
                    values.Add(ObfuscateValue(element, depth + 1, ref remainingValues));
                }
            }
        }
        catch (Exception)
        {
            return _secretObfuscator.Obfuscate("[Unavailable log value]", null);
        }

        // Always snapshot: even a currently harmless collection could be mutated before export.
        return values.ToArray();
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
