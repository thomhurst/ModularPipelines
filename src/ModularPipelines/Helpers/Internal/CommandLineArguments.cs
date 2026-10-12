using System.Text;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Formats arguments the way the runtime pastes <see cref="System.Diagnostics.ProcessStartInfo.ArgumentList"/>
/// into a command line, so displayed and masked input matches what the process receives.
/// </summary>
internal static class CommandLineArguments
{
    public static string Join(IEnumerable<string> arguments)
    {
        var builder = new StringBuilder();
        foreach (var argument in arguments)
        {
            if (builder.Length > 0)
            {
                builder.Append(' ');
            }

            AppendEscaped(builder, argument);
        }

        return builder.ToString();
    }

    public static string Escape(string argument)
    {
        var builder = new StringBuilder(argument.Length + 2);
        AppendEscaped(builder, argument);
        return builder.ToString();
    }

    // Follows the Microsoft C runtime parsing rules: backslashes are literal unless they precede a quote,
    // so a run of backslashes is doubled before a quote or the closing quote.
    private static void AppendEscaped(StringBuilder builder, string argument)
    {
        if (argument.Length > 0 && !argument.Any(static c => char.IsWhiteSpace(c) || c == '"'))
        {
            builder.Append(argument);
            return;
        }

        builder.Append('"');
        var index = 0;
        while (index < argument.Length)
        {
            var character = argument[index++];
            if (character == '\\')
            {
                var backslashCount = 1;
                while (index < argument.Length && argument[index] == '\\')
                {
                    index++;
                    backslashCount++;
                }

                if (index == argument.Length)
                {
                    builder.Append('\\', backslashCount * 2);
                }
                else if (argument[index] == '"')
                {
                    builder.Append('\\', (backslashCount * 2) + 1).Append('"');
                    index++;
                }
                else
                {
                    builder.Append('\\', backslashCount);
                }
            }
            else if (character == '"')
            {
                builder.Append('\\').Append('"');
            }
            else
            {
                builder.Append(character);
            }
        }

        builder.Append('"');
    }
}
