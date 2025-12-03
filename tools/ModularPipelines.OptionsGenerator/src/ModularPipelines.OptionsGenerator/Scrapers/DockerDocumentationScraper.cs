using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;

namespace ModularPipelines.OptionsGenerator.Scrapers;

/// <summary>
/// Scrapes Docker CLI documentation from docs.docker.com.
/// Docker docs use hierarchical URLs like /reference/cli/docker/container/run/.
/// </summary>
public partial class DockerDocumentationScraper : CliDocumentationScraperBase
{
    private const string BaseUrl = "https://docs.docker.com/reference/cli/docker/";

    public override string ToolName => "docker";
    public override string NamespacePrefix => "Docker";
    public override string TargetNamespace => "ModularPipelines.Docker";
    public override string OutputDirectory => "src/ModularPipelines.Docker";

    // Command categories to scrape
    private static readonly string[] CommandCategories =
    [
        "container", "image", "network", "volume", "system",
        "buildx", "compose", "config", "context", "manifest",
        "node", "plugin", "secret", "service", "stack", "swarm", "trust"
    ];

    // Top-level commands (not in a category)
    private static readonly string[] TopLevelCommands =
    [
        "build", "login", "logout", "pull", "push", "run", "exec",
        "ps", "images", "create", "start", "stop", "restart", "kill",
        "rm", "rmi", "tag", "search", "version", "info", "inspect"
    ];

    public DockerDocumentationScraper(HttpClient httpClient, ILogger<DockerDocumentationScraper> logger)
        : base(httpClient, logger)
    {
    }

    public override async Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default)
    {
        var commands = new List<CliCommandDefinition>();
        var errors = new List<ScrapingError>();

        Logger.LogInformation("Fetching Docker CLI reference from {Url}", BaseUrl);

        // Scrape top-level commands first
        foreach (var cmd in TopLevelCommands)
        {
            var url = $"{BaseUrl}{cmd}/";
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
                // Command might not have its own page - that's okay
                Logger.LogDebug("No dedicated page for docker {Cmd}", cmd);
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

        // Scrape command categories
        foreach (var category in CommandCategories)
        {
            var categoryUrl = $"{BaseUrl}{category}/";
            try
            {
                var subCommands = await ScrapeCommandCategoryAsync(categoryUrl, category, cancellationToken);
                commands.AddRange(subCommands);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to scrape category {Category}", category);
                errors.Add(new ScrapingError
                {
                    Source = categoryUrl,
                    Message = ex.Message,
                    Exception = ex
                });
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

    private async Task<List<CliCommandDefinition>> ScrapeCommandCategoryAsync(
        string categoryUrl,
        string category,
        CancellationToken cancellationToken)
    {
        var commands = new List<CliCommandDefinition>();

        try
        {
            var doc = await FetchHtmlAsync(categoryUrl, cancellationToken);

            // Find all subcommand links
            var links = doc.QuerySelectorAll($"a[href*='/reference/cli/docker/{category}/']");

            var processedUrls = new HashSet<string>();

            foreach (var link in links)
            {
                var href = link.GetAttribute("href");
                if (string.IsNullOrEmpty(href))
                    continue;

                var fullUrl = href.StartsWith("http")
                    ? href
                    : $"https://docs.docker.com{href}";

                // Skip if already processed or is the category page itself
                if (!processedUrls.Add(fullUrl) ||
                    fullUrl.TrimEnd('/').EndsWith($"/{category}"))
                    continue;

                try
                {
                    var commandParts = ExtractCommandParts(fullUrl, category);
                    if (commandParts.Length == 0)
                        continue;

                    var command = await ScrapeCommandPageAsync(fullUrl, commandParts, cancellationToken);
                    if (command is not null)
                    {
                        commands.Add(command);
                        Logger.LogDebug("Scraped command: {Command}", command.FullCommand);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Failed to scrape {Url}", fullUrl);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to fetch category page {Url}", categoryUrl);
        }

        return commands;
    }

    private static string[] ExtractCommandParts(string url, string category)
    {
        // /reference/cli/docker/container/run/ -> ["container", "run"]
        var match = CommandUrlPattern().Match(url);
        if (!match.Success)
            return [];

        var path = match.Groups[1].Value.Trim('/');
        return path.Split('/', StringSplitOptions.RemoveEmptyEntries);
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

        // Determine sub-domain group
        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;

        var className = GenerateClassName(commandParts);

        return new CliCommandDefinition
        {
            FullCommand = $"docker {string.Join(" ", commandParts)}",
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = "DockerOptions",
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = url,
            Options = options,
            PositionalArguments = ExtractPositionalArguments(doc),
            SubDomainGroup = subDomain,
            Enums = options.Where(o => o.EnumDefinition is not null).Select(o => o.EnumDefinition!).ToList()
        };
    }

    private static string? ExtractDescription(IDocument doc)
    {
        // Docker docs typically have description in first paragraph or meta description
        var meta = doc.QuerySelector("meta[name='description']");
        if (meta is not null)
        {
            var content = meta.GetAttribute("content");
            if (!string.IsNullOrEmpty(content))
                return content;
        }

        var content2 = doc.QuerySelector("article p, .content p, main p");
        return content2?.TextContent.Trim();
    }

    private List<CliOptionDefinition> ExtractOptions(IDocument doc, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var className = GenerateClassName(commandParts);

        // Docker docs use tables for options
        var tables = doc.QuerySelectorAll("table");

        foreach (var table in tables)
        {
            // Check if this is an options table by looking at headers
            var headers = table.QuerySelectorAll("th, thead td");
            var headerText = string.Join(" ", headers.Select(h => h.TextContent.ToLowerInvariant()));

            if (!headerText.Contains("option") && !headerText.Contains("flag"))
                continue;

            var rows = table.QuerySelectorAll("tbody tr, tr").Skip(headers.Any() ? 0 : 1);
            foreach (var row in rows)
            {
                var cells = row.QuerySelectorAll("td").ToArray();
                if (cells.Length < 2)
                    continue;

                var option = ParseOptionRow(cells, className);
                if (option is not null && !options.Any(o => o.SwitchName == option.SwitchName))
                {
                    options.Add(option);
                }
            }
        }

        // Also parse from code blocks and definition lists
        var codeBlocks = doc.QuerySelectorAll("pre code, .highlight code");
        foreach (var block in codeBlocks)
        {
            var matches = OptionPattern().Matches(block.TextContent);
            foreach (Match match in matches)
            {
                var longForm = match.Groups["long"].Value.Trim();
                if (string.IsNullOrEmpty(longForm) || options.Any(o => o.SwitchName == longForm))
                    continue;

                var shortForm = match.Groups["short"].Value.Trim();
                var valueType = match.Groups["type"].Value.Trim();
                var description = match.Groups["desc"].Value.Trim();

                var switchName = longForm;
                var propertyName = NormalizePropertyName(longForm);
                if (propertyName is null)
                    continue;

                var isFlag = DetectBooleanFlag(description, valueType);
                var isNumeric = DetectNumericType(valueType);
                var acceptsMultiple = DetectMultipleValues(description, valueType);

                var enumDef = DetectEnumValues(propertyName, className, valueType, description);
                var csharpType = DetermineCSharpType(isFlag, acceptsMultiple, false, isNumeric, enumDef);

                options.Add(new CliOptionDefinition
                {
                    SwitchName = switchName,
                    ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                    PropertyName = propertyName,
                    CSharpType = csharpType,
                    Description = description,
                    IsFlag = isFlag,
                    IsRequired = false,
                    AcceptsMultipleValues = acceptsMultiple,
                    IsKeyValue = false,
                    IsNumeric = isNumeric,
                    ValueSeparator = isFlag ? " " : "=",
                    EnumDefinition = enumDef
                });
            }
        }

        return options;
    }

    private CliOptionDefinition? ParseOptionRow(IElement[] cells, string className)
    {
        if (cells.Length < 2)
            return null;

        var optionCell = cells[0].TextContent.Trim();
        var descriptionCell = cells.Length > 1 ? cells[1].TextContent.Trim() : string.Empty;
        var defaultCell = cells.Length > 2 ? cells[2].TextContent.Trim() : null;

        // Parse option names (can be "-d, --detach" format)
        var parts = optionCell.Split(',', StringSplitOptions.TrimEntries);
        string? shortForm = null;
        string? longForm = null;

        foreach (var part in parts)
        {
            var cleaned = part.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
            if (cleaned.StartsWith("--"))
                longForm = cleaned;
            else if (cleaned.StartsWith("-"))
                shortForm = cleaned;
        }

        if (string.IsNullOrEmpty(longForm))
        {
            // If no long form, use short form as the switch
            if (string.IsNullOrEmpty(shortForm))
                return null;
            longForm = shortForm;
            shortForm = null;
        }

        var propertyName = NormalizePropertyName(longForm);
        if (propertyName is null)
            return null;

        var isFlag = DetectBooleanFlag(descriptionCell, defaultCell);
        var isNumeric = DetectNumericType(defaultCell);
        var acceptsMultiple = DetectMultipleValues(descriptionCell, defaultCell);

        var enumDef = DetectEnumValues(propertyName, className, defaultCell, descriptionCell);
        var csharpType = DetermineCSharpType(isFlag, acceptsMultiple, false, isNumeric, enumDef);

        return new CliOptionDefinition
        {
            SwitchName = longForm,
            ShortForm = shortForm,
            PropertyName = propertyName,
            CSharpType = csharpType,
            Description = descriptionCell,
            IsFlag = isFlag,
            IsRequired = false,
            AcceptsMultipleValues = acceptsMultiple,
            IsKeyValue = false,
            IsNumeric = isNumeric,
            ValueSeparator = isFlag ? " " : "=",
            EnumDefinition = enumDef
        };
    }

    private static List<CliPositionalArgument> ExtractPositionalArguments(IDocument doc)
    {
        // Docker commands often have positional args like IMAGE, CONTAINER
        // For now, return empty - can be enhanced later
        return [];
    }

    [GeneratedRegex(@"/reference/cli/docker/(.+?)/?$")]
    private static partial Regex CommandUrlPattern();

    [GeneratedRegex(@"(?:(?<short>-\w),\s*)?(?<long>--[\w-]+)(?:\s+(?<type>[^\s]+))?\s+(?<desc>[^\n]+)?", RegexOptions.Multiline)]
    private static partial Regex OptionPattern();
}
