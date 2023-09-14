namespace ModularPipelines.Engine;

internal interface ISecretProvider
{
    IReadOnlyList<string> Secrets { get; }
    IEnumerable<string> GetSecretsInObject(object? option);
}