namespace ModularPipelines.Engine;

internal interface ISecretProvider
{
    string[] Secrets { get; }
    IEnumerable<string> GetSecrets(object? option);
}