using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Docker.
/// Docker is a Cobra-based CLI with consistent help formatting.
/// </summary>
public class DockerCliScraper : CobraCliScraper
{
    private static readonly CliOptionDefinition ComposeExecNoTtyOption = new()
    {
        SwitchName = "--no-TTY",
        ShortForm = "-T",
        PropertyName = "NoTty",
        CSharpType = "bool?",
        Description = "Disable pseudo-TTY allocation (default: auto-detected)",
        IsFlag = false,
        ValueSeparator = "=",
    };

    public override string ToolName => "docker";
    public override string NamespacePrefix => "Docker";
    public override string TargetNamespace => "ModularPipelines.Docker";
    public override string OutputDirectory => "src/ModularPipelines.Docker";

    /// <summary>
    /// Docker commands can dispatch to separate CLI plugins. Keep concurrent plugin
    /// processes bounded so command discovery does not exhaust runner memory.
    /// </summary>
    protected override int MaxParallelism => 4;

    public DockerCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<DockerCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    /// <inheritdoc />
    protected override CliCommandGroupAlias? DetectCommandGroupAlias(
        string[] commandPath,
        string helpText) =>
        DockerCliCompatibility.DetectCommandGroupAlias(commandPath, helpText);

    /// <inheritdoc />
    protected override string NormalizeOptionSwitchName(
        string[] commandParts,
        string switchName) =>
        commandParts is ["compose", ..]
        && switchName.Equals("--no-tty", StringComparison.OrdinalIgnoreCase)
            ? "--no-TTY"
            : switchName;

    /// <inheritdoc />
    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts,
        IReadOnlyList<CliOptionDefinition> options)
    {
        if (commandParts is not ["compose", "exec"]
            || options.Any(option => option.PropertyName == ComposeExecNoTtyOption.PropertyName))
        {
            return options;
        }

        return [.. options, ComposeExecNoTtyOption];
    }
}
