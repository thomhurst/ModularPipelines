using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers;

internal static class DotNetCliCompatibility
{
    private static readonly HashSet<string> NoLogoCommands =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "build",
            "clean",
            "pack",
            "publish",
        };

    private static readonly CliCompatibilityProperty NoLogoProperty = new()
    {
        PropertyName = "Nologo",
        CSharpType = "bool?",
        ForwardToPropertyName = "NoLogo",
        ObsoleteMessage = "Use NoLogo instead.",
    };

    private static readonly IReadOnlyList<CliCompatibilityProperty> NoLogoProperties =
    [
        NoLogoProperty,
    ];

    private static readonly IReadOnlyList<CliCompatibilityProperty> BuildProperties =
    [
        NoLogoProperty,
        new CliCompatibilityProperty
        {
            PropertyName = "Debug",
            CSharpType = "bool?",
            ObsoleteMessage = "The dotnet --debug switch is no longer supported and this property has no effect.",
        },
    ];

    public static void NormalizeOptions(IReadOnlyList<string> commandParts, List<CliOptionDefinition> options)
    {
        if (!SupportsNoLogoCompatibility(commandParts))
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

    public static IReadOnlyList<CliCompatibilityProperty> GetProperties(IReadOnlyList<string> commandParts) =>
        IsBuildCommand(commandParts)
            ? BuildProperties
            : SupportsNoLogoCompatibility(commandParts)
                ? NoLogoProperties
                : [];

    private static bool IsBuildCommand(IReadOnlyList<string> commandParts) =>
        commandParts.Count == 1 && commandParts[0].Equals("build", StringComparison.OrdinalIgnoreCase);

    private static bool SupportsNoLogoCompatibility(IReadOnlyList<string> commandParts) =>
        commandParts.Count == 1 && NoLogoCommands.Contains(commandParts[0]);
}
