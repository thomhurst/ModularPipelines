using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Helm.
/// Helm is a Cobra-based CLI with consistent help formatting.
/// </summary>
public class HelmCliScraper : CobraCliScraper
{
    public override string ToolName => "helm";
    public override string NamespacePrefix => "Helm";
    public override string TargetNamespace => "ModularPipelines.Helm";
    public override string OutputDirectory => "src/ModularPipelines.Helm";

    protected override string VersionArguments => "version";

    public HelmCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<HelmCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (commandParts is not ["install"])
        {
            return positionalArguments;
        }

        return positionalArguments
            .Select(argument => argument.PropertyName.Equals("Chart", StringComparison.OrdinalIgnoreCase)
                ? argument with
                {
                    CSharpType = argument.CSharpType.TrimEnd('?'),
                    IsRequired = true,
                }
                : argument)
            .ToList();
    }
}
