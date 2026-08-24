using System.Text.RegularExpressions;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers;

internal static partial class DockerCliAliases
{
    private static readonly CliCommandGroupAlias BuilderAlias = new()
    {
        Alias = "builder",
        CanonicalCommand = "buildx",
        ObsoleteMessage = "docker builder is an alias of docker buildx. Use Buildx instead.",
    };

    public static IReadOnlyList<CliCommandGroupAlias> CommandGroupAliases => [BuilderAlias];

    public static IReadOnlyList<CliCommandGroupAlias> GetSupportedCommandGroupAliases(
        IReadOnlyCollection<CliCommandDefinition> commands)
    {
        return CommandGroupAliases
            .Where(alias => commands.Any(command =>
                command.CommandParts.FirstOrDefault()?.Equals(
                    alias.CanonicalCommand,
                    StringComparison.OrdinalIgnoreCase) == true))
            .ToArray();
    }

    public static CliCommandGroupAlias? DetectCommandGroupAlias(
        IReadOnlyList<string> commandPath,
        string helpText)
    {
        if (commandPath.Count != 2
            || !commandPath[0].Equals("docker", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var match = UsageCommandGroupPattern().Match(helpText);
        if (!match.Success)
        {
            return null;
        }

        var requestedCommand = commandPath[1];
        var canonicalCommand = match.Groups["command"].Value;
        return requestedCommand.Equals(canonicalCommand, StringComparison.OrdinalIgnoreCase)
            ? null
            : CommandGroupAliases.SingleOrDefault(alias =>
                alias.Alias.Equals(requestedCommand, StringComparison.OrdinalIgnoreCase)
                && alias.CanonicalCommand.Equals(canonicalCommand, StringComparison.OrdinalIgnoreCase));
    }

    [GeneratedRegex(
        @"^[ \t]*Usage:?[ \t]*(?:\r?\n[ \t]*)?docker[ \t]+(?<command>[a-z0-9-]+)(?:[ \t]|$)",
        RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex UsageCommandGroupPattern();
}
