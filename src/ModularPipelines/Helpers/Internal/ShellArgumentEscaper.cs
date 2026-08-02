namespace ModularPipelines.Helpers.Internal;

internal static class ShellArgumentEscaper
{
    public static string Escape(string argument) =>
        "'" + argument.Replace("'", "'\\''", StringComparison.Ordinal) + "'";
}
