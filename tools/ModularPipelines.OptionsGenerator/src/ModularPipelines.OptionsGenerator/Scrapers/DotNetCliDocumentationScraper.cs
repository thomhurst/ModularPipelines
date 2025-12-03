using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;

namespace ModularPipelines.OptionsGenerator.Scrapers;

/// <summary>
/// Scrapes .NET CLI documentation from learn.microsoft.com.
/// .NET docs use URLs like /dotnet/core/tools/dotnet-[command].
/// </summary>
public partial class DotNetCliDocumentationScraper : CliDocumentationScraperBase
{
    private const string BaseUrl = "https://learn.microsoft.com/en-us/dotnet/core/tools/";
    private const string IndexUrl = $"{BaseUrl}dotnet";

    public override string ToolName => "dotnet";
    public override string NamespacePrefix => "DotNet";
    public override string TargetNamespace => "ModularPipelines.DotNet";
    public override string OutputDirectory => "src/ModularPipelines.DotNet";

    // Core .NET CLI commands to scrape
    private static readonly string[] CoreCommands =
    [
        "build", "clean", "new", "pack", "publish", "restore", "run", "test",
        "build-server", "format", "watch", "msbuild", "vstest", "workload"
    ];

    // Commands with subcommands
    private static readonly string[] CommandsWithSubcommands =
    [
        "nuget", "tool", "package", "reference", "sln", "workload", "sdk"
    ];

    // Subcommand mappings
    private static readonly Dictionary<string, string[]> SubCommands = new()
    {
        ["nuget"] = ["delete", "locals", "push", "add source", "disable source", "enable source", "list source", "remove source", "update source", "verify", "sign", "trust"],
        ["tool"] = ["install", "list", "restore", "run", "search", "uninstall", "update"],
        ["package"] = ["add", "list", "remove", "search"],
        ["reference"] = ["add", "list", "remove"],
        ["sln"] = ["add", "list", "remove"],
        ["workload"] = ["install", "list", "repair", "restore", "search", "uninstall", "update"],
        ["sdk"] = ["check"]
    };

    public DotNetCliDocumentationScraper(HttpClient httpClient, ILogger<DotNetCliDocumentationScraper> logger)
        : base(httpClient, logger)
    {
    }

    public override async Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default)
    {
        var commands = new List<CliCommandDefinition>();
        var errors = new List<ScrapingError>();

        Logger.LogInformation("Fetching .NET CLI reference from {Url}", IndexUrl);

        // Scrape core commands
        foreach (var cmd in CoreCommands)
        {
            var url = $"{BaseUrl}dotnet-{cmd}";
            try
            {
                var command = await ScrapeCommandPageAsync(url, [cmd], cancellationToken);
                if (command is not null)
                {
                    commands.Add(command);
                    Logger.LogDebug("Scraped command: {Command}", command.FullCommand);
                }
            }
            catch (HttpRequestException)
            {
                Logger.LogDebug("No dedicated page for dotnet {Cmd}", cmd);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to scrape {Url}", url);
                errors.Add(new ScrapingError
                {
                    Source = url,
                    Message = ex.Message,
                    Exception = ex
                });
            }
        }

        // Scrape commands with subcommands
        foreach (var (parent, subs) in SubCommands)
        {
            foreach (var sub in subs)
            {
                var subParts = sub.Split(' ');
                var urlSuffix = string.Join("-", [parent, ..subParts]);
                var url = $"{BaseUrl}dotnet-{urlSuffix}";

                try
                {
                    var commandParts = new[] { parent }.Concat(subParts).ToArray();
                    var command = await ScrapeCommandPageAsync(url, commandParts, cancellationToken);
                    if (command is not null)
                    {
                        commands.Add(command);
                        Logger.LogDebug("Scraped command: {Command}", command.FullCommand);
                    }
                }
                catch (HttpRequestException)
                {
                    Logger.LogDebug("No dedicated page for dotnet {Parent} {Sub}", parent, sub);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Failed to scrape {Url}", url);
                    errors.Add(new ScrapingError
                    {
                        Source = url,
                        Message = ex.Message,
                        Exception = ex
                    });
                }
            }
        }

        return new CliToolDefinition
        {
            ToolName = ToolName,
            NamespacePrefix = NamespacePrefix,
            TargetNamespace = TargetNamespace,
            OutputDirectory = OutputDirectory,
            Commands = commands,
            Errors = errors
        };
    }

    private async Task<CliCommandDefinition?> ScrapeCommandPageAsync(
        string url,
        string[] commandParts,
        CancellationToken cancellationToken)
    {
        var doc = await FetchHtmlAsync(url, cancellationToken);

        // Get description
        var description = ExtractDescription(doc);

        // Parse options
        var options = ExtractOptions(doc, commandParts);

        // Parse positional arguments (like project path)
        var positionalArgs = ExtractPositionalArguments(doc);

        if (options.Count == 0 && positionalArgs.Count == 0)
            return null;

        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;
        var className = GenerateClassName(commandParts);

        return new CliCommandDefinition
        {
            FullCommand = $"dotnet {string.Join(" ", commandParts)}",
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = "DotNetOptions",
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = url,
            Options = options,
            PositionalArguments = positionalArgs,
            SubDomainGroup = subDomain,
            Enums = options.Where(o => o.EnumDefinition is not null).Select(o => o.EnumDefinition!).ToList()
        };
    }

    private static string? ExtractDescription(IDocument doc)
    {
        // .NET docs have description in first paragraph
        var meta = doc.QuerySelector("meta[name='description']");
        if (meta is not null)
        {
            var content = meta.GetAttribute("content");
            if (!string.IsNullOrEmpty(content))
                return content;
        }

        var firstPara = doc.QuerySelector("article p, .content p, main p");
        return firstPara?.TextContent.Trim();
    }

    private List<CliOptionDefinition> ExtractOptions(IDocument doc, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var className = GenerateClassName(commandParts);

        // .NET docs use definition lists for options
        var dls = doc.QuerySelectorAll("dl");

        foreach (var dl in dls)
        {
            var dts = dl.QuerySelectorAll("dt").ToArray();
            var dds = dl.QuerySelectorAll("dd").ToArray();

            for (int i = 0; i < dts.Length; i++)
            {
                var dt = dts[i];
                var dd = i < dds.Length ? dds[i] : null;

                var option = ParseDefinitionListItem(dt, dd, className);
                if (option is not null && !options.Any(o => o.SwitchName == option.SwitchName))
                {
                    options.Add(option);
                }
            }
        }

        // Also look for tables with options
        var tables = doc.QuerySelectorAll("table");
        foreach (var table in tables)
        {
            var headers = table.QuerySelectorAll("th");
            var headerText = string.Join(" ", headers.Select(h => h.TextContent.ToLowerInvariant()));

            if (!headerText.Contains("option") && !headerText.Contains("argument"))
                continue;

            var rows = table.QuerySelectorAll("tbody tr, tr").Skip(headers.Any() ? 0 : 1);
            foreach (var row in rows)
            {
                var cells = row.QuerySelectorAll("td").ToArray();
                if (cells.Length < 2)
                    continue;

                var option = ParseTableRow(cells, className);
                if (option is not null && !options.Any(o => o.SwitchName == option.SwitchName))
                {
                    options.Add(option);
                }
            }
        }

        // Parse from usage code blocks
        var codeBlocks = doc.QuerySelectorAll("pre code");
        foreach (var block in codeBlocks)
        {
            var text = block.TextContent;
            if (!text.Contains("dotnet") && !text.Contains("--"))
                continue;

            var matches = OptionPattern().Matches(text);
            foreach (Match match in matches)
            {
                var shortForm = match.Groups["short"].Value.Trim();
                var longForm = match.Groups["long"].Value.Trim();
                var valueType = match.Groups["type"].Value.Trim();

                if (string.IsNullOrEmpty(longForm) || options.Any(o => o.SwitchName == longForm))
                    continue;

                var propertyName = NormalizePropertyName(longForm);
                if (propertyName is null)
                    continue;

                var isFlag = string.IsNullOrEmpty(valueType) ||
                             valueType.Contains("bool", StringComparison.OrdinalIgnoreCase);

                options.Add(new CliOptionDefinition
                {
                    SwitchName = longForm,
                    ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                    PropertyName = propertyName,
                    CSharpType = isFlag ? "bool?" : "string?",
                    Description = null,
                    IsFlag = isFlag,
                    IsRequired = false,
                    AcceptsMultipleValues = false,
                    IsKeyValue = false,
                    IsNumeric = false,
                    ValueSeparator = " ",
                    EnumDefinition = null
                });
            }
        }

        return options;
    }

    private CliOptionDefinition? ParseDefinitionListItem(IElement dt, IElement? dd, string className)
    {
        var text = dt.TextContent.Trim();
        var description = dd?.TextContent.Trim();

        // Parse option names from format like "-c|--configuration <CONFIGURATION>"
        var match = OptionDefinitionPattern().Match(text);
        if (!match.Success)
            return null;

        var shortForm = match.Groups["short"].Value.Trim();
        var longForm = match.Groups["long"].Value.Trim();
        var valueType = match.Groups["type"].Value.Trim();

        if (string.IsNullOrEmpty(longForm))
        {
            if (string.IsNullOrEmpty(shortForm))
                return null;
            longForm = shortForm;
            shortForm = null;
        }

        var propertyName = NormalizePropertyName(longForm);
        if (propertyName is null)
            return null;

        var isFlag = string.IsNullOrEmpty(valueType) ||
                     description?.Contains("boolean", StringComparison.OrdinalIgnoreCase) == true;
        var isNumeric = description?.Contains("integer", StringComparison.OrdinalIgnoreCase) == true ||
                        valueType.Contains("int", StringComparison.OrdinalIgnoreCase);
        var acceptsMultiple = description?.Contains("multiple", StringComparison.OrdinalIgnoreCase) == true ||
                              description?.Contains("can be specified more than once", StringComparison.OrdinalIgnoreCase) == true;

        var enumDef = DetectEnumValues(propertyName, className, valueType, description);
        var csharpType = DetermineCSharpType(isFlag, acceptsMultiple, false, isNumeric, enumDef);

        return new CliOptionDefinition
        {
            SwitchName = longForm,
            ShortForm = shortForm,
            PropertyName = propertyName,
            CSharpType = csharpType,
            Description = description,
            IsFlag = isFlag,
            IsRequired = false,
            AcceptsMultipleValues = acceptsMultiple,
            IsKeyValue = false,
            IsNumeric = isNumeric,
            ValueSeparator = " ",
            EnumDefinition = enumDef
        };
    }

    private CliOptionDefinition? ParseTableRow(IElement[] cells, string className)
    {
        if (cells.Length < 2)
            return null;

        var optionCell = cells[0].TextContent.Trim();
        var description = cells.Length > 1 ? cells[1].TextContent.Trim() : string.Empty;

        // Parse option pattern
        var match = OptionDefinitionPattern().Match(optionCell);
        if (!match.Success)
            return null;

        var shortForm = match.Groups["short"].Value.Trim();
        var longForm = match.Groups["long"].Value.Trim();
        var valueType = match.Groups["type"].Value.Trim();

        if (string.IsNullOrEmpty(longForm))
        {
            if (string.IsNullOrEmpty(shortForm))
                return null;
            longForm = shortForm;
            shortForm = null;
        }

        var propertyName = NormalizePropertyName(longForm);
        if (propertyName is null)
            return null;

        var isFlag = string.IsNullOrEmpty(valueType);
        var isNumeric = description.Contains("integer", StringComparison.OrdinalIgnoreCase);
        var acceptsMultiple = description.Contains("multiple", StringComparison.OrdinalIgnoreCase);

        var enumDef = DetectEnumValues(propertyName, className, valueType, description);
        var csharpType = DetermineCSharpType(isFlag, acceptsMultiple, false, isNumeric, enumDef);

        return new CliOptionDefinition
        {
            SwitchName = longForm,
            ShortForm = shortForm,
            PropertyName = propertyName,
            CSharpType = csharpType,
            Description = description,
            IsFlag = isFlag,
            IsRequired = false,
            AcceptsMultipleValues = acceptsMultiple,
            IsKeyValue = false,
            IsNumeric = isNumeric,
            ValueSeparator = " ",
            EnumDefinition = enumDef
        };
    }

    private static List<CliPositionalArgument> ExtractPositionalArguments(IDocument doc)
    {
        var args = new List<CliPositionalArgument>();

        // .NET CLI commands have very few true positional arguments
        // Most commonly: PROJECT, SOLUTION, or PACKAGE_NAME
        // We need to be selective to avoid picking up option value placeholders
        var validPositionalArgs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "PROJECT", "SOLUTION", "PACKAGE_NAME", "PACKAGE_ID", "TOOL_ID",
            "TEMPLATE_NAME", "SEARCH_TERM", "COMMAND", "PATH_TO_SLN"
        };

        var codeBlocks = doc.QuerySelectorAll("pre code");

        foreach (var block in codeBlocks)
        {
            var text = block.TextContent;
            if (!text.Contains("dotnet"))
                continue;

            // Only look at the first usage line
            var lines = text.Split('\n');
            var usageLine = lines.FirstOrDefault(l => l.TrimStart().StartsWith("dotnet"));
            if (string.IsNullOrEmpty(usageLine))
                continue;

            // Look for angle-bracket patterns that are NOT preceded by --
            var matches = PositionalArgPattern().Matches(usageLine);
            var index = 0;

            foreach (Match match in matches)
            {
                var argName = match.Groups[1].Value;

                // Check if this is preceded by a flag (meaning it's an option value, not positional)
                var matchIndex = match.Index;
                var precedingText = usageLine[..matchIndex];
                if (precedingText.EndsWith("--") || precedingText.TrimEnd().EndsWith("-"))
                    continue;

                // Only include known positional argument types
                if (!validPositionalArgs.Contains(argName))
                    continue;

                var propertyName = NormalizePropertyName(argName);

                if (!args.Any(a => a.PropertyName == propertyName))
                {
                    args.Add(new CliPositionalArgument
                    {
                        PlaceholderName = argName,
                        PropertyName = propertyName,
                        CSharpType = "string?",
                        Description = null,
                        Placement = PositionalArgumentPosition.AfterOptions,
                        PositionIndex = index++,
                        IsRequired = false
                    });
                }
            }

            // Only process first usage block
            break;
        }

        return args;
    }

    [GeneratedRegex(@"(?:(?<short>-\w)\|)?(?<long>--[\w-]+)(?:\s+<(?<type>[^>]+)>)?", RegexOptions.IgnoreCase)]
    private static partial Regex OptionDefinitionPattern();

    [GeneratedRegex(@"(?:(?<short>-\w)\s*[|,]\s*)?(?<long>--[\w-]+)(?:\s+(?<type>[^\s]+))?", RegexOptions.Multiline)]
    private static partial Regex OptionPattern();

    [GeneratedRegex(@"<([A-Z_]+)>", RegexOptions.Compiled)]
    private static partial Regex PositionalArgPattern();
}
