namespace ModularPipelines.Engine;

internal interface IBuildSystemSecretMasker
{
    void MaskSecrets(IEnumerable<string> secrets);
}