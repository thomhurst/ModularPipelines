using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers;

internal static class DotNetCliCompatibility
{
    private static readonly IReadOnlyList<CliCompatibilityProperty> BuildProperties =
    [
        new CliCompatibilityProperty
        {
            PropertyName = "Nologo",
            CSharpType = "bool?",
            ForwardToPropertyName = "NoLogo",
            ObsoleteMessage = "Use NoLogo instead.",
        },
        new CliCompatibilityProperty
        {
            PropertyName = "Debug",
            CSharpType = "bool?",
            ObsoleteMessage = "The dotnet --debug switch is no longer supported and this property has no effect.",
        },
    ];

    public static void NormalizeOptions(IReadOnlyList<string> commandParts, List<CliOptionDefinition> options)
    {
        if (!IsBuildCommand(commandParts))
        {
            return;
        }

        options.RemoveAll(option => option.SwitchName.Equals("--debug", StringComparison.OrdinalIgnoreCase));

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
        IsBuildCommand(commandParts) ? BuildProperties : [];

    private static bool IsBuildCommand(IReadOnlyList<string> commandParts) =>
        commandParts.Count == 1 && commandParts[0].Equals("build", StringComparison.OrdinalIgnoreCase);
}
