using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers;

internal static class DotNetCliNormalizer
{
    private static readonly HashSet<string> NoLogoCommands =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "build",
            "clean",
            "pack",
            "publish",
        };

    public static void NormalizeOptions(IReadOnlyList<string> commandParts, List<CliOptionDefinition> options)
    {
        if (!SupportsNoLogo(commandParts))
        {
            return;
        }

        if (IsBuildCommand(commandParts))
        {
            options.RemoveAll(option => option.SwitchName.Equals("--debug", StringComparison.OrdinalIgnoreCase));
        }

        var noLogoIndex = options.FindIndex(option =>
            option.SwitchName.Equals("--nologo", StringComparison.OrdinalIgnoreCase) ||
            option.SwitchName.Equals("--no-logo", StringComparison.OrdinalIgnoreCase));
        if (noLogoIndex >= 0)
        {
            options[noLogoIndex] = options[noLogoIndex] with
            {
                SwitchName = "--nologo",
                ShortForm = null,
                PropertyName = "NoLogo",
            };
        }
    }

    private static bool IsBuildCommand(IReadOnlyList<string> commandParts) =>
        commandParts.Count == 1 && commandParts[0].Equals("build", StringComparison.OrdinalIgnoreCase);

    private static bool SupportsNoLogo(IReadOnlyList<string> commandParts) =>
        commandParts.Count == 1 && NoLogoCommands.Contains(commandParts[0]);
}
