namespace ModularPipelines.Engine;

internal interface ISecretProvider
{
    /// <summary>
    /// Gets a list of the detected secrets from IOptions objects.
    /// </summary>
    IEnumerable<string> Secrets { get; }

    /// <summary>
    /// Returns any values in the object marked with the [SecretValue] attribute.
    /// </summary>
    /// <param name="value">Object to check for secret values within its properties.</param>
    /// <returns>Array of secrets.</returns>
    IEnumerable<string> GetSecretsInObject(object? value);
}