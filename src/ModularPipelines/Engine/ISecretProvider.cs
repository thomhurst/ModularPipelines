namespace ModularPipelines.Engine;

internal interface ISecretProvider
{
    IEnumerable<string> Secrets { get; }
    IEnumerable<string> GetSecretsInObject(object? option);
}