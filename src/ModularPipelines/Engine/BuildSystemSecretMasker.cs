namespace ModularPipelines.Engine;

internal class BuildSystemSecretMasker : IBuildSystemSecretMasker
{
    private readonly IBuildSystemDetector _buildSystemDetector;
    private readonly IConsoleWriter _consoleWriter;

    private readonly List<string> _alreadyMaskedSecrets = new();
    private readonly object _lock = new();

    public BuildSystemSecretMasker(IBuildSystemDetector buildSystemDetector,
        IConsoleWriter consoleWriter)
    {
        _buildSystemDetector = buildSystemDetector;
        _consoleWriter = consoleWriter;
    }

    public void MaskSecrets(IEnumerable<string> secrets)
    {
        lock (_lock)
        {
            foreach (var secret in secrets.Where(s => !_alreadyMaskedSecrets.Contains(s)))
            {
                _alreadyMaskedSecrets.Add(secret);

                if (_buildSystemDetector.IsRunningOnGitHubActions)
                {
                    _consoleWriter.LogToConsole($"::add-mask::{secret}");
                }
            }
        }
    }
}