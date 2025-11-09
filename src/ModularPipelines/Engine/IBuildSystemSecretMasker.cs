namespace ModularPipelines.Engine;

/// <summary>
/// Interface for masking secrets in build system logs.
/// </summary>
internal interface IBuildSystemSecretMasker
{
    /// <summary>
    /// Sends commands to the build system to mask the specified secrets in logs.
    /// Prevents sensitive values from appearing in plain text in CI/CD output.
    /// </summary>
    /// <param name="secrets">The secret values to mask.</param>
    /// <remarks>
    /// The masking mechanism varies by build system:
    /// - GitHub Actions: Uses ::add-mask::
    /// - Azure Pipelines: Uses ##vso[task.setvariable] with issecret=true
    /// - TeamCity: Uses ##teamcity[setParameter] with system.password prefix
    /// - Other systems: May not support runtime masking
    /// </remarks>
    void MaskSecrets(IEnumerable<string> secrets);
}
