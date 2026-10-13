using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Helm.
/// Helm is a Cobra-based CLI with consistent help formatting.
/// </summary>
public class HelmCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<HelmCliScraper> logger) : CobraCliScraper(executor, helpCache, logger)
{
    public override string ToolName => "helm";
    public override string NamespacePrefix => "Helm";
    public override string TargetNamespace => "ModularPipelines.Helm";
    public override string OutputDirectory => "src/ModularPipelines.Helm";

    protected override string VersionArguments => "version";

    protected override bool GlobalOptionsBeforeSubcommands => false;

    // Helm registers its settings on root.PersistentFlags(); --help is Cobra's local flag.
    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        [.. ParseNamedOptionSection(helpText, "Flags", []).Where(option => option.SwitchName != "--help")];

    protected override IReadOnlyList<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts) =>
        ParseOptionsWithoutRootGlobals(helpText, commandParts);

    protected override string NormalizeOptionDescription(string description)
    {
        // These defaults contain platform-dependent user directories. Keep generated docs reproducible.
        if (description.StartsWith("path to the registry config file (default ", StringComparison.Ordinal)
            || description.StartsWith("path to the directory containing cached repository indexes (default ", StringComparison.Ordinal)
            || description.StartsWith("path to the file containing repository names and URLs (default ", StringComparison.Ordinal))
        {
            return description[..description.IndexOf(" (default ", StringComparison.Ordinal)];
        }

        return description;
    }

    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (commandParts is not ["install"])
        {
            return positionalArguments;
        }

        return [.. positionalArguments
            .Select(argument => argument.PropertyName.Equals("Chart", StringComparison.OrdinalIgnoreCase)
                ? argument with
                {
                    CSharpType = argument.CSharpType.TrimEnd('?'),
                    IsRequired = true,
                }
                : argument)];
    }
}
