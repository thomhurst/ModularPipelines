using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;

namespace ModularPipelines.OptionsGenerator.Scrapers;

/// <summary>
/// Scrapes Helm CLI documentation from helm.sh.
/// Helm docs use underscore-separated URLs like helm_install, helm_repo_add.
/// </summary>
public partial class HelmDocumentationScraper : CliDocumentationScraperBase
{
    private const string BaseUrl = "https://helm.sh/docs/helm/helm/";
    private const string CommandBaseUrl = "https://helm.sh/docs/helm/";

    public override string ToolName => "helm";
    public override string NamespacePrefix => "Helm";
    public override string TargetNamespace => "ModularPipelines.Helm";
    public override string OutputDirectory => "src/ModularPipelines.Helm";

    public HelmDocumentationScraper(HttpClient httpClient, ILogger<HelmDocumentationScraper> logger)
        : base(httpClient, logger)
    {
    }

    public override async Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default)
    {
        var commands = new List<CliCommandDefinition>();
        var errors = new List<ScrapingError>();

        Logger.LogInformation("Fetching Helm command index from {Url}", BaseUrl);

        try
        {
            var mainDoc = await FetchHtmlAsync(BaseUrl, cancellationToken);

            // Find all command links in the sidebar/navigation
            var commandLinks = ExtractCommandLinks(mainDoc);
            Logger.LogInformation("Found {Count} command links", commandLinks.Count);

            foreach (var (url, commandName) in commandLinks)
            {
                try
                {
                    var command = await ScrapeCommandPageAsync(url, commandName, cancellationToken);
                    if (command is not null)
                    {
                        commands.Add(command);
                        Logger.LogDebug("Scraped command: {Command}", command.FullCommand);
                    }
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
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to fetch Helm command index");
            errors.Add(new ScrapingError
            {
                Source = BaseUrl,
                Message = ex.Message,
                Exception = ex
            });
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

    private List<(string Url, string CommandName)> ExtractCommandLinks(IDocument doc)
    {
        var links = new List<(string, string)>();

        // Find links that match the Helm command pattern
        var allLinks = doc.QuerySelectorAll("a[href*='/docs/helm/helm_']");

        foreach (var link in allLinks)
        {
            var href = link.GetAttribute("href");
            if (string.IsNullOrEmpty(href))
                continue;

            // Normalize URL
            var fullUrl = href.StartsWith("http")
                ? href
                : $"https://helm.sh{href}";

            // Extract command name from URL
            // /docs/helm/helm_install/ -> helm_install
            var match = CommandUrlPattern().Match(fullUrl);
            if (match.Success)
            {
                var commandName = match.Groups[1].Value;
                if (!links.Any(l => l.Item2 == commandName))
                {
                    links.Add((fullUrl.TrimEnd('/') + "/", commandName));
                }
            }
        }

        return links;
    }

    private async Task<CliCommandDefinition?> ScrapeCommandPageAsync(
        string url,
        string commandName,
        CancellationToken cancellationToken)
    {
        var doc = await FetchHtmlAsync(url, cancellationToken);

        // Parse command parts from name (helm_repo_add -> ["repo", "add"])
        var commandParts = ParseCommandParts(commandName);
        if (commandParts.Length == 0)
            return null;

        // Determine sub-domain group (first command part)
        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;

        // Get description from page
        var description = ExtractDescription(doc);

        // Parse options from the page
        var options = ExtractOptions(doc, commandParts);

        // Find positional arguments from synopsis
        var positionalArgs = ExtractPositionalArguments(doc);

        // Build enum definitions from options
        var enums = options
            .Where(o => o.EnumDefinition is not null)
            .Select(o => o.EnumDefinition!)
            .ToList();

        var className = GenerateClassName(commandParts);

        return new CliCommandDefinition
        {
            FullCommand = $"helm {string.Join(" ", commandParts)}",
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = "HelmOptions",
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = url,
            Options = options,
            PositionalArguments = positionalArgs,
            SubDomainGroup = subDomain,
            Enums = enums
        };
    }

    private static string[] ParseCommandParts(string commandName)
    {
        // helm_repo_add -> ["repo", "add"]
        var parts = commandName.Split('_', StringSplitOptions.RemoveEmptyEntries);

        // Skip "helm" prefix
        return parts.Length > 1 ? parts[1..] : [];
    }

    private static string? ExtractDescription(IDocument doc)
    {
        // Look for the first paragraph after the title, or synopsis section
        var content = doc.QuerySelector("article, .content, main");
        if (content is null)
            return null;

        // Find synopsis or first meaningful paragraph
        var paragraphs = content.QuerySelectorAll("p");
        foreach (var p in paragraphs)
        {
            var text = p.TextContent.Trim();
            if (!string.IsNullOrEmpty(text) &&
                !text.StartsWith("Auto generated") &&
                text.Length > 20)
            {
                return text;
            }
        }

        return null;
    }

    private List<CliOptionDefinition> ExtractOptions(IDocument doc, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var className = GenerateClassName(commandParts);

        // Find the Options section - usually in a <pre> or code block after "Options" heading
        var content = doc.QuerySelector("article, .content, main");
        if (content is null)
            return options;

        var fullText = content.TextContent;

        // Parse options using regex to find patterns like:
        // --flag-name type    description
        // -f, --flag-name type    description
        var optionMatches = OptionPattern().Matches(fullText);

        foreach (Match match in optionMatches)
        {
            var shortForm = match.Groups["short"].Value.Trim();
            var longForm = match.Groups["long"].Value.Trim();
            var valueType = match.Groups["type"].Value.Trim();
            var description = match.Groups["desc"].Value.Trim();

            if (string.IsNullOrEmpty(longForm))
                continue;

            var switchName = longForm;
            var propertyName = NormalizePropertyName(longForm);
            if (propertyName is null)
                continue;

            // Detect if this is a boolean flag
            var isFlag = DetectBooleanFlag(description, valueType);
            var isNumeric = DetectNumericType(valueType);
            var acceptsMultiple = DetectMultipleValues(description, valueType) ||
                                  valueType.Contains("Array", StringComparison.OrdinalIgnoreCase);

            // Detect enum values
            var enumDef = DetectEnumValues(propertyName, className, valueType, description);

            var csharpType = DetermineCSharpType(isFlag, acceptsMultiple, false, isNumeric, enumDef);

            // Determine separator (Helm uses = for most flags)
            var separator = isFlag ? " " : "=";

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
                ValueSeparator = separator,
                EnumDefinition = enumDef
            });
        }

        return options;
    }

    private static List<CliPositionalArgument> ExtractPositionalArguments(IDocument doc)
    {
        // Helm commands typically don't have complex positional args
        // Most are like: helm install [NAME] [CHART]
        // For now, return empty - can be enhanced later by parsing synopsis
        return [];
    }

    [GeneratedRegex(@"/docs/helm/(helm_[\w_]+)/?")]
    private static partial Regex CommandUrlPattern();

    [GeneratedRegex(@"(?:(?<short>-\w),\s*)?(?<long>--[\w-]+)\s+(?<type>\w+(?:Array)?)?(?:\s{2,}|\t)(?<desc>[^\n]+)", RegexOptions.Multiline)]
    private static partial Regex OptionPattern();
}
