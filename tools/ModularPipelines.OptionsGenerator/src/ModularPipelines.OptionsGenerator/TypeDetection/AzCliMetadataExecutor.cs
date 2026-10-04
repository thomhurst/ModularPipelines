using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>Obtains help and argument arity from the same installed Azure CLI parser.</summary>
internal sealed partial class AzCliMetadataExecutor(ICliCommandExecutor inner) : ICliCommandExecutor
{
    internal const string MetadataMarker = "__MODULAR_PIPELINES_AZ_ARGUMENT_FLAGS__:";

    private readonly ConcurrentDictionary<string, Lazy<Task<string>>> _pythonPaths = new(StringComparer.Ordinal);

    // Invoking --help loads the selected command's argparse actions without executing it.
    // Keep help on stdout so the normal cache and provenance capture both help and metadata.
    private const string Script = """
        import json, sys
        from azure.cli.core import get_default_cli
        cli = get_default_cli()
        try:
            code = cli.invoke(sys.argv[1:])
        except SystemExit as exit:
            code = exit.code
        if code not in (None, 0):
            sys.exit(code)
        parser = cli.invocation.parser
        command = ' '.join(sys.argv[1:-1])
        selected = parser.subparser_map.get(command, parser)
        flags = {option: action.nargs == 0 for action in selected._actions for option in action.option_strings}
        print('\n__MODULAR_PIPELINES_AZ_ARGUMENT_FLAGS__:' + json.dumps(flags, sort_keys=True))
        """;

    public async Task<CliCommandResult> ExecuteAsync(
        string command,
        string arguments,
        CancellationToken cancellationToken = default,
        string? workingDirectory = null)
    {
        if (!arguments.EndsWith("--help", StringComparison.Ordinal))
        {
            return await inner.ExecuteAsync(command, arguments, cancellationToken, workingDirectory).ConfigureAwait(false);
        }

        var python = await _pythonPaths.GetOrAdd(command,
                executable => new Lazy<Task<string>>(() => ResolvePythonAsync(executable, cancellationToken)))
            .Value.WaitAsync(cancellationToken).ConfigureAwait(false);
        var encodedScript = Convert.ToBase64String(Encoding.UTF8.GetBytes(Script));
        var result = await inner.ExecuteAsync(python,
                $"-c \"import base64;exec(base64.b64decode('{encodedScript}'))\" {arguments}",
                cancellationToken, workingDirectory)
            .ConfigureAwait(false);
        if (!result.Unavailable && (!result.Success || !result.StandardOutput.Contains(MetadataMarker, StringComparison.Ordinal)))
        {
            throw new InvalidOperationException("Azure CLI help did not include verified argument arity metadata.");
        }

        return result;
    }

    public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
        inner.IsAvailableAsync(command, cancellationToken);

    private async Task<string> ResolvePythonAsync(string command, CancellationToken cancellationToken)
    {
        try
        {
            var version = await inner.ExecuteAsync(command, "--version", cancellationToken).ConfigureAwait(false);
            var match = PythonLocationPattern().Match(version.CombinedOutput);
            if (version.Unavailable || !version.Success || !match.Success)
            {
                throw new InvalidOperationException("Azure CLI did not report its Python location; argument arity cannot be verified.");
            }

            return match.Groups["path"].Value;
        }
        catch
        {
            // Only the cached lookup removes its entry, before completing with failure.
            // Individual callers cancelling WaitAsync cannot evict a live lookup or a replacement.
            _pythonPaths.TryRemove(command, out _);
            throw;
        }
    }

    [GeneratedRegex("Python location ['\\\"](?<path>[^'\\\"]+)['\\\"]")]
    private static partial Regex PythonLocationPattern();
}
