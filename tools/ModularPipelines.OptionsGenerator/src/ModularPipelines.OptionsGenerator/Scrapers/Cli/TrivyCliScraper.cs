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
        "--help", "-h", "--version", "help", "completion", "version"
    };

    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (commandParts is ["plugin", "upgrade"] or ["vex", "repo", "download"])
        {
            return CreateOptionalNameArguments(commandParts, positionalArguments);
        }

        if (commandParts is ["plugin", "run"])
        {
            return CreatePluginRunArguments(positionalArguments);
        }

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

        return CreateMissingPositionalArguments(commandParts, positionalArguments);
    }

    private static IReadOnlyList<CliPositionalArgument> CreateOptionalNameArguments(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        var defaultArgument = commandParts is ["plugin", "upgrade"]
            ? OptionalArgument("PluginNames")
            : OptionalArgument("RepoNames");
        var arguments = positionalArguments.Count > 0
            ? positionalArguments
            : [defaultArgument];

        return arguments
            .Select(argument => argument with
            {
                CSharpType = "IEnumerable<string>?",
                IsRequired = false,
            })
            .ToList();
    }

    private static IReadOnlyList<CliPositionalArgument> CreatePluginRunArguments(
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        var sourceArgument = positionalArguments.Count > 0
            ? positionalArguments[0] with { PropertyName = "Source" }
            : RequiredArgument("Source");

        return
        [
            sourceArgument with { PositionIndex = 0 },
            OptionalArgument("PluginArguments") with
            {
                CSharpType = "IEnumerable<string>?",
                Placement = PositionalArgumentPosition.AfterOptions,
                PositionIndex = 1,
            },
        ];
    }

    private static IReadOnlyList<CliPositionalArgument> CreateMissingPositionalArguments(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments) =>
        commandParts switch
        {
            ["config"] => [RequiredArgument("Directory")],
            ["filesystem"] => [RequiredArgument("Path")],
            ["image"] => [OptionalArgument("ImageName")],
            ["repository"] => [RequiredArgument("Repository")],
            ["rootfs"] => [RequiredArgument("RootDirectory")],
            ["sbom"] => [RequiredArgument("SbomPath")],
            ["vm"] => [RequiredArgument("VmImage")],
            ["convert"] => [RequiredArgument("ResultJson")],
            ["module", "install"] or ["module", "uninstall"] =>
                [RequiredArgument("Repository")],
            ["plugin", "info"] or ["plugin", "uninstall"] =>
                [RequiredArgument("PluginName")],
            ["plugin", "install"] =>
                [RequiredArgument("Source")],
            ["registry", "login"] or ["registry", "logout"] =>
                [RequiredArgument("Server")],
            _ => positionalArguments,
        };

    protected override string NormalizeOptionDescription(string description)
    {
        var normalizedDescription = UserHomeDirectoryPattern().Replace(description, "<home>");
        return TrivyCacheDirectoryPattern().Replace(normalizedDescription, "<cache>/trivy");
    }

    private static CliPositionalArgument RequiredArgument(string propertyName) => new()
    {
        PropertyName = propertyName,
        CSharpType = "string",
        IsRequired = true,
        PositionIndex = 0,
    };

    private static CliPositionalArgument OptionalArgument(string propertyName) =>
        RequiredArgument(propertyName) with
        {
            CSharpType = "string?",
            IsRequired = false,
        };

    [GeneratedRegex(@"(?i)(?:[A-Z]:[\\/]+Users[\\/]+|/(?:home|Users)/)[^\\/\s\""')]+")]
    private static partial Regex UserHomeDirectoryPattern();

    [GeneratedRegex(@"(?i)<home>(?:[\\/]+AppData[\\/]+Local|[\\/]+\.cache|[\\/]+Library[\\/]+Caches)[\\/]+trivy")]
    private static partial Regex TrivyCacheDirectoryPattern();
}
