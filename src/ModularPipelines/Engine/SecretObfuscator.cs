using System.Text;

namespace ModularPipelines.Engine;

internal class SecretObfuscator : ISecretObfuscator
{
    private readonly ISecretProvider _secretProvider;
    private readonly string[] _secrets;

    public SecretObfuscator(IBuildSystemSecretMasker buildSystemSecretMasker,
        ISecretProvider secretProvider)
    {
        _secretProvider = secretProvider;
        _secrets = secretProvider.Secrets;
        buildSystemSecretMasker.MaskSecrets(_secrets);
    }

    public string Obfuscate(string input, object? optionsObject)
    {
        var stringBuilder = new StringBuilder(input);

        var secretsFromExtraObject = _secretProvider.GetSecrets(optionsObject);

        foreach (var secret in _secrets.Concat(secretsFromExtraObject))
        {
            if (input.Contains(secret))
            {
                stringBuilder.Replace(secret, "**********");
            }
        }

        return stringBuilder.ToString();
    }

  
}