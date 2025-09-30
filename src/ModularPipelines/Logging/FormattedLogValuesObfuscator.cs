using System.Reflection;
using ModularPipelines.Engine;

namespace ModularPipelines.Logging;

/// <summary>
/// Handles obfuscation of values within FormattedLogValues objects using reflection.
/// This class intercepts Microsoft.Extensions.Logging internal structures to mask secrets.
/// </summary>
/// <remarks>
/// This class uses reflection to access the internal "_values" field of FormattedLogValues,
/// which is an implementation detail of Microsoft.Extensions.Logging. It applies secret
/// obfuscation to all string values before they are logged, preventing sensitive data leakage.
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
    private const string FormattedLogValuesTypeName = "Microsoft.Extensions.Logging.FormattedLogValues";
    private const string ValuesFieldName = "_values";

    private readonly ISecretObfuscator _secretObfuscator;

    public FormattedLogValuesObfuscator(ISecretObfuscator secretObfuscator)
    {
        _secretObfuscator = secretObfuscator;
    }

    public void TryObfuscateValues(object state)
    {
        if (state?.GetType().FullName != FormattedLogValuesTypeName)
        {
            return;
        }

        var valuesField = state.GetType().GetField(ValuesFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (valuesField == null)
        {
            return;
        }

        var values = valuesField.GetValue(state) as object?[] ?? Array.Empty<object?>();

        for (var index = 0; index < values.Length; index++)
        {
            var value = values[index];
            if (value is null)
            {
                continue;
            }

            var valueString = value.ToString() ?? string.Empty;
            values[index] = _secretObfuscator.Obfuscate(valueString, null);
        }
    }
}

/// <summary>
/// Interface for obfuscating formatted log values.
/// </summary>
internal interface IFormattedLogValuesObfuscator
{
    /// <summary>
    /// Attempts to obfuscate values within a FormattedLogValues object.
    /// </summary>
    /// <param name="state">The state object to obfuscate.</param>
    void TryObfuscateValues(object state);
}