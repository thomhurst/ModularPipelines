namespace ModularPipelines.Engine;

/// <summary>
/// Masks secrets in build system logs by sending appropriate commands to the build system.
/// Supports GitHub Actions, Azure Pipelines, TeamCity, and other CI/CD systems.
/// </summary>
/// <remarks>
/// Different build systems have different mechanisms for masking secrets:
/// - GitHub Actions: ::add-mask::
/// - Azure Pipelines: ##vso[task.setvariable variable=X;issecret=true]
/// - TeamCity: ##teamcity[setParameter name='system.password.X' value='...']
/// - GitLab, Jenkins, etc.: Handled via configuration, no runtime command
/// </remarks>
/// <example>
/// <code>
/// var masker = new BuildSystemSecretMasker(formatterProvider, consoleWriter);
///
/// // Mask multiple secrets
/// masker.MaskSecrets(new[] { "password123", "api-key-xyz" });
///
/// // Build system will receive appropriate masking commands
/// // GitHub: "::add-mask::password123"
/// // Azure: "##vso[task.setvariable variable=secret_abc;issecret=true]password123"
/// </code>
/// </example>
internal class BuildSystemSecretMasker : IBuildSystemSecretMasker
{
    private readonly IBuildSystemFormatterProvider _formatterProvider;
    private readonly IConsoleWriter _consoleWriter;

    private readonly List<string> _alreadyMaskedSecrets = new();
    private readonly object _lock = new();

    public BuildSystemSecretMasker(IBuildSystemFormatterProvider formatterProvider,
        IConsoleWriter consoleWriter)
    {
        _formatterProvider = formatterProvider;
        _consoleWriter = consoleWriter;
    }

    public void MaskSecrets(IEnumerable<string> secrets)
    {
        lock (_lock)
        {
            var formatter = _formatterProvider.GetFormatter();

            foreach (var secret in secrets.Where(s => !_alreadyMaskedSecrets.Contains(s)))
            {
                _alreadyMaskedSecrets.Add(secret);

                var maskCommand = formatter.GetMaskSecretCommand(secret);
                if (maskCommand != null)
                {
                    _consoleWriter.LogToConsole(maskCommand);
                }
            }
        }
    }
}
