namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Formatter for Travis CI build system.
/// Supports log folding via travis_fold commands.
/// </summary>
/// <remarks>
/// Travis CI uses special commands for log folding.
/// Documentation: https://docs.travis-ci.com/user/customizing-the-build/#customizing-the-build-step
/// </remarks>
/// <example>
/// <code>
/// // Example output:
/// // travis_fold:start:build_step
/// // ... log content ...
/// // travis_fold:end:build_step
/// </code>
/// </example>
internal class TravisCIFormatter : IBuildSystemFormatter
{
    public string GetStartBlockCommand(string name)
    {
        var foldId = SanitizeFoldId(name);
        return $"travis_fold:start:{foldId}\r\x1b[33;1m{name}\x1b[0m";
    }

    public string GetEndBlockCommand(string name)
    {
        var foldId = SanitizeFoldId(name);
        return $"travis_fold:end:{foldId}";
    }

    public string? GetMaskSecretCommand(string secret)
    {
        // Travis CI automatically masks encrypted variables
        return null;
    }

    private static string SanitizeFoldId(string name)
    {
        return string.Concat(name.Where(c => char.IsLetterOrDigit(c) || c == '_')).ToLowerInvariant();
    }
}
