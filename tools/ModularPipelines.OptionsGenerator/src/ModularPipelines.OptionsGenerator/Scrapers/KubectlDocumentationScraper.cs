using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;

namespace ModularPipelines.OptionsGenerator.Scrapers;

/// <summary>
/// Scrapes kubectl CLI documentation from kubernetes.io.
/// Kubectl docs are on a single page with anchor links for each command.
/// </summary>
public partial class KubectlDocumentationScraper : CliDocumentationScraperBase
{
    private const string BaseUrl = "https://kubernetes.io/docs/reference/generated/kubectl/kubectl-commands/";

    public override string ToolName => "kubectl";
    public override string NamespacePrefix => "Kubectl";
    public override string TargetNamespace => "ModularPipelines.Kubernetes";
    public override string OutputDirectory => "src/ModularPipelines.Kubernetes";

    public KubectlDocumentationScraper(HttpClient httpClient, ILogger<KubectlDocumentationScraper> logger)
        : base(httpClient, logger)
    {
    }

    public override async Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default)
    {
        var commands = new List<CliCommandDefinition>();
        var errors = new List<ScrapingError>();

        Logger.LogInformation("Fetching kubectl command reference from {Url}", BaseUrl);

        try
        {
            var doc = await FetchHtmlAsync(BaseUrl, cancellationToken);

            // Find all command sections (they have specific heading patterns)
            var commandSections = ExtractCommandSections(doc);
            Logger.LogInformation("Found {Count} command sections", commandSections.Count);

            foreach (var section in commandSections)
            {
                try
                {
                    var command = ParseCommandSection(section);
                    if (command is not null)
                    {
                        commands.Add(command);
                        Logger.LogDebug("Parsed command: {Command}", command.FullCommand);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Failed to parse command section");
                    errors.Add(new ScrapingError
                    {
                        Source = BaseUrl,
                        Message = $"Failed to parse command section: {ex.Message}",
                        Exception = ex
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to fetch kubectl command reference");
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

    private List<CommandSection> ExtractCommandSections(IDocument doc)
    {
        var sections = new List<CommandSection>();

        // Get the main content area
        var content = doc.QuerySelector("article, .content, main, #content, body");
        if (content is null)
            return sections;

        // kubectl docs use <h1> elements for main commands (apply, get, etc.)
        var headings = content.QuerySelectorAll("h1");

        foreach (var heading in headings)
        {
            var headingText = heading.TextContent.Trim();

            // Skip non-command headings (empty or document title)
            if (string.IsNullOrEmpty(headingText) ||
                headingText.Equals("kubectl", StringComparison.OrdinalIgnoreCase) ||
                headingText.Contains("Reference", StringComparison.OrdinalIgnoreCase) ||
                headingText.Contains("Documentation", StringComparison.OrdinalIgnoreCase))
                continue;

            // Skip if heading doesn't look like a command name (should be a single word or hyphenated)
            if (headingText.Contains(" ") && !headingText.Contains("-"))
                continue;

            // Collect all content until the next h1
            var sectionContent = new List<IElement>();
            var sibling = heading.NextElementSibling;
            while (sibling is not null && sibling.TagName.ToUpperInvariant() != "H1")
            {
                sectionContent.Add(sibling);
                sibling = sibling.NextElementSibling;
            }

            if (sectionContent.Count > 0)
            {
                sections.Add(new CommandSection
                {
                    Heading = headingText,
                    HeadingId = heading.Id ?? heading.GetAttribute("id"),
                    ContentElements = sectionContent
                });
            }
        }

        return sections;
    }

    private CliCommandDefinition? ParseCommandSection(CommandSection section)
    {
        // Heading is just the command name (e.g., "apply", "get", "config")
        var commandName = section.Heading.Trim().ToLowerInvariant();

        // Parse command parts (handles hyphenated subcommands like "config-set-context")
        var commandParts = commandName.Split('-', StringSplitOptions.RemoveEmptyEntries);

        if (commandParts.Length == 0 || string.IsNullOrEmpty(commandParts[0]))
            return null;

        // Get description from first paragraph
        var description = ExtractDescription(section.ContentElements);

        // Parse options from the section
        var options = ExtractOptions(section.ContentElements, commandParts);

        // Determine sub-domain group
        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;

        var className = GenerateClassName(commandParts);

        return new CliCommandDefinition
        {
            FullCommand = $"kubectl {string.Join(" ", commandParts)}",
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = "KubernetesOptions",
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = $"{BaseUrl}#{section.HeadingId}",
            Options = options,
            PositionalArguments = [],
            SubDomainGroup = subDomain,
            Enums = options.Where(o => o.EnumDefinition is not null).Select(o => o.EnumDefinition!).ToList()
        };
    }

    private static string? ExtractDescription(List<IElement> elements)
    {
        foreach (var element in elements)
        {
            if (element.TagName.ToUpperInvariant() == "P")
            {
                var text = element.TextContent.Trim();
                if (!string.IsNullOrEmpty(text) && text.Length > 20)
                {
                    return text;
                }
            }
        }

        return null;
    }

    private List<CliOptionDefinition> ExtractOptions(List<IElement> elements, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var className = GenerateClassName(commandParts);

        // Look for tables containing flags
        foreach (var element in elements)
        {
            var tables = element.TagName.ToUpperInvariant() == "TABLE"
                ? new[] { element }
                : element.QuerySelectorAll("table").ToArray();

            foreach (var table in tables)
            {
                var rows = table.QuerySelectorAll("tr").Skip(1); // Skip header
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
        }

        // Also look for options in preformatted text (code blocks)
        foreach (var element in elements)
        {
            var text = element.TextContent;
            var matches = OptionPattern().Matches(text);

            foreach (Match match in matches)
            {
                var longForm = match.Groups["long"].Value.Trim();
                var shortForm = match.Groups["short"].Value.Trim();
                var valueType = match.Groups["type"].Value.Trim();
                var description = match.Groups["desc"].Value.Trim();

                if (string.IsNullOrEmpty(longForm) || options.Any(o => o.SwitchName == longForm))
                    continue;

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

    private CliOptionDefinition? ParseTableRow(IElement[] cells, string className)
    {
        // Expected format: Name, Shorthand, Default, Usage
        if (cells.Length < 2)
            return null;

        var nameCell = cells[0].TextContent.Trim();
        var shorthand = cells.Length > 1 ? cells[1].TextContent.Trim() : null;
        var defaultValue = cells.Length > 2 ? cells[2].TextContent.Trim() : null;
        var usage = cells.Length > 3 ? cells[3].TextContent.Trim() : string.Empty;

        if (string.IsNullOrEmpty(nameCell) || !nameCell.StartsWith("--"))
            return null;

        var switchName = nameCell;
        var propertyName = NormalizePropertyName(nameCell);
        if (propertyName is null)
            return null;

        // Detect types from default value and usage
        var isFlag = string.Equals(defaultValue, "false", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(defaultValue, "true", StringComparison.OrdinalIgnoreCase) ||
                     usage.Contains("true", StringComparison.OrdinalIgnoreCase) && !usage.Contains("=");

        var isNumeric = int.TryParse(defaultValue, out _) && !isFlag;
        var acceptsMultiple = usage.Contains("[]") || usage.Contains("can be specified multiple", StringComparison.OrdinalIgnoreCase);

        var enumDef = DetectEnumValues(propertyName, className, defaultValue, usage);
        var csharpType = DetermineCSharpType(isFlag, acceptsMultiple, false, isNumeric, enumDef);

        return new CliOptionDefinition
        {
            SwitchName = switchName,
            ShortForm = string.IsNullOrEmpty(shorthand) || shorthand == "-" ? null : (shorthand.StartsWith("-") ? shorthand : $"-{shorthand}"),
            PropertyName = propertyName,
            CSharpType = csharpType,
            Description = usage,
            IsFlag = isFlag,
            IsRequired = false,
            AcceptsMultipleValues = acceptsMultiple,
            IsKeyValue = false,
            IsNumeric = isNumeric,
            ValueSeparator = isFlag ? " " : "=",
            EnumDefinition = enumDef
        };
    }

    private record CommandSection
    {
        public required string Heading { get; init; }
        public string? HeadingId { get; init; }
        public required List<IElement> ContentElements { get; init; }
    }

    [GeneratedRegex(@"(?:(?<short>-\w),\s*)?(?<long>--[\w-]+)(?:\s+(?<type>[^\s]+))?\s+(?<desc>[^\n]+)", RegexOptions.Multiline)]
    private static partial Regex OptionPattern();
}
