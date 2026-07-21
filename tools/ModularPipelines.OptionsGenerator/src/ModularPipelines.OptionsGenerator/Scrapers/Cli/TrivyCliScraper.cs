using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Trivy security scanner.
/// Trivy uses Cobra-style help format (Go-based CLI).
///
/// trivy help format (trivy --help):
/// Scanner for vulnerabilities in container images, file systems, and Git repositories, as well as for configuration issues and hard-coded secrets
///
/// Usage:
///   trivy [global flags] command [flags] target
///   trivy [command]
///
/// Scanning Commands:
///   config      Scan config files for misconfigurations
///   filesystem  Scan local filesystem
///   image       Scan a container image
///   repository  Scan a repository
///   rootfs      Scan rootfs
///   sbom        Scan SBOM for vulnerabilities and licenses
///   vm          [EXPERIMENTAL] Scan a virtual machine image
///
/// Management Commands:
///   module      Manage modules
///   plugin      Manage plugins
///   server      Server mode
///
/// Utility Commands:
///   clean       Remove cached files
///   convert     Convert Trivy JSON report into a different format
///   version     Print the version
/// </summary>
public partial class TrivyCliScraper : CobraCliScraper
{
    public TrivyCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<TrivyCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "trivy";

    public override string NamespacePrefix => "Trivy";

    public override string TargetNamespace => "ModularPipelines.Trivy";

    public override string OutputDirectory => "src/ModularPipelines.Trivy";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "clean"
    };

    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (commandParts is ["image"] && positionalArguments.Count > 0)
        {
            return positionalArguments
                .Select(argument => argument with { CSharpType = "string?", IsRequired = false })
                .ToList();
        }

        if (positionalArguments.Count > 0)
        {
            return positionalArguments;
        }

        return commandParts switch
        {
            ["config"] => [RequiredArgument("Directory", "DIR")],
            ["filesystem"] => [RequiredArgument("Path", "PATH")],
            ["image"] => [OptionalArgument("ImageName", "IMAGE_NAME")],
            ["repository"] => [RequiredArgument("Repository", "REPO_PATH | REPO_URL")],
            ["rootfs"] => [RequiredArgument("RootDirectory", "ROOTDIR")],
            ["sbom"] => [RequiredArgument("SbomPath", "SBOM_PATH")],
            ["vm"] => [RequiredArgument("VmImage", "VM_IMAGE")],
            ["convert"] => [RequiredArgument("ResultJson", "RESULT_JSON")],
            ["module", "install"] or ["module", "uninstall"] =>
                [RequiredArgument("Repository", "REPOSITORY")],
            ["plugin", "info"] or ["plugin", "uninstall"] =>
                [RequiredArgument("PluginName", "PLUGIN_NAME")],
            ["plugin", "install"] or ["plugin", "run"] =>
                [RequiredArgument("Source", "NAME | URL | FILE_PATH")],
            ["registry", "login"] or ["registry", "logout"] =>
                [RequiredArgument("Server", "SERVER")],
            _ => positionalArguments,
        };
    }

    protected override string NormalizeOptionDescription(string description) =>
        UserHomeDirectoryPattern().Replace(description, "<home>");

    private static CliPositionalArgument RequiredArgument(string propertyName, string placeholderName) => new()
    {
        PropertyName = propertyName,
        PlaceholderName = placeholderName,
        CSharpType = "string",
        IsRequired = true,
        PositionIndex = 0,
    };

    private static CliPositionalArgument OptionalArgument(string propertyName, string placeholderName) =>
        RequiredArgument(propertyName, placeholderName) with
        {
            CSharpType = "string?",
            IsRequired = false,
        };

    [GeneratedRegex(@"(?i)(?:[A-Z]:[\\/]+Users[\\/]+|/(?:home|Users)/)[^\\/\s\""')]+")]
    private static partial Regex UserHomeDirectoryPattern();
}
