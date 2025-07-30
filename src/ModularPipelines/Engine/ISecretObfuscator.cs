namespace ModularPipelines.Engine;

/// <summary>
/// Provides functionality to obfuscate sensitive information in logs and output.
/// </summary>
public interface ISecretObfuscator
{
    /// <summary>
    /// Obfuscates sensitive information in the provided input.
    /// </summary>
    /// <param name="input">The input string that may contain sensitive information.</param>
    /// <param name="optionsObject">An options object that may contain sensitive properties.</param>
    /// <returns>The input with sensitive information obfuscated.</returns>
    string Obfuscate(string? input, object? optionsObject);
}