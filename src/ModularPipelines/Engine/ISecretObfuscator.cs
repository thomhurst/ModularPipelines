namespace ModularPipelines.Engine;

public interface ISecretObfuscator
{
    string Obfuscate(string? input, object? optionsObject);
}