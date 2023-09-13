﻿using System.Text;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Engine;

internal class SecretObfuscator : ISecretObfuscator, IInitializer
{
    private readonly IBuildSystemSecretMasker _buildSystemSecretMasker;
    private readonly ISecretProvider _secretProvider;

    public SecretObfuscator(IBuildSystemSecretMasker buildSystemSecretMasker,
        ISecretProvider secretProvider)
    {
        _buildSystemSecretMasker = buildSystemSecretMasker;
        _secretProvider = secretProvider;
    }

    public string Obfuscate(string input, object? optionsObject)
    {
        var stringBuilder = new StringBuilder(input);

        var secretsFromExtraObject = _secretProvider.GetSecretsInObject(optionsObject);

        foreach (var secret in _secretProvider.Secrets.Concat(secretsFromExtraObject))
        {
            if (input.Contains(secret))
            {
                stringBuilder.Replace(secret, "**********");
            }
        }

        return stringBuilder.ToString();
    }

    public Task InitializeAsync()
    {
        _buildSystemSecretMasker.MaskSecrets(_secretProvider.Secrets);
        return Task.CompletedTask;
    }

    public int Order => int.MaxValue;
}