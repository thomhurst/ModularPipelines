using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;

namespace ModularPipelines.OptionsGenerator.Scrapers;

/// <summary>
/// Scrapes Azure CLI documentation from learn.microsoft.com.
/// Azure CLI docs use URLs like /cli/azure/[command-group]?view=azure-cli-latest.
/// </summary>
public partial class AzureCliDocumentationScraper : CliDocumentationScraperBase
{
    private const string BaseUrl = "https://learn.microsoft.com/en-us/cli/azure/reference-index?view=azure-cli-latest";
    private const string CommandBaseUrl = "https://learn.microsoft.com/en-us/cli/azure/";

    public override string ToolName => "az";
    public override string NamespacePrefix => "Az";
    public override string TargetNamespace => "ModularPipelines.Azure";
    public override string OutputDirectory => "src/ModularPipelines.Azure";

    // Priority command groups to focus on (most commonly used)
    // Limit to top 15 to avoid excessive scraping
    private static readonly string[] PriorityGroups =
    [
        "account", "group", "resource", "storage", "vm", "webapp",
        "aks", "acr", "keyvault", "network", "sql", "cosmosdb",
        "functionapp", "container", "appservice"
    ];

    // Maximum subcommands to fetch per group to avoid timeout
    private const int MaxSubCommandsPerGroup = 5;

    public AzureCliDocumentationScraper(HttpClient httpClient, ILogger<AzureCliDocumentationScraper> logger)
        : base(httpClient, logger)
    {
    }

    public override async Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default)
    {
        var commands = new List<CliCommandDefinition>();
        var errors = new List<ScrapingError>();

        Logger.LogInformation("Fetching Azure CLI reference index from {Url}", BaseUrl);

        try
        {
            var indexDoc = await FetchHtmlAsync(BaseUrl, cancellationToken);

            // Extract command groups from the index
            var commandGroups = ExtractCommandGroups(indexDoc);
            Logger.LogInformation("Found {Count} command groups", commandGroups.Count);

            // Only process priority groups to avoid excessive scraping
            var priorityGroupsToProcess = commandGroups
                .Where(g => PriorityGroups.Contains(g.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();

            Logger.LogInformation("Processing {Count} priority groups", priorityGroupsToProcess.Count);

            foreach (var group in priorityGroupsToProcess)
            {
                try
                {
                    var groupCommands = await ScrapeCommandGroupAsync(group, cancellationToken);
                    commands.AddRange(groupCommands);
                    Logger.LogInformation("Scraped {Count} commands from {Group}", groupCommands.Count, group.Name);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Failed to scrape command group {Group}", group.Name);
                    errors.Add(new ScrapingError
                    {
                        Source = group.Url,
                        Message = ex.Message,
                        Exception = ex
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to fetch Azure CLI reference index");
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

    private List<CommandGroup> ExtractCommandGroups(IDocument doc)
    {
        var groups = new List<CommandGroup>();

        // Azure docs have tables with command groups
        var tables = doc.QuerySelectorAll("table");

        foreach (var table in tables)
        {
            var rows = table.QuerySelectorAll("tbody tr, tr").Skip(1); // Skip header
            foreach (var row in rows)
            {
                var cells = row.QuerySelectorAll("td").ToArray();
                if (cells.Length < 2)
                    continue;

                var nameCell = cells[0];
                var link = nameCell.QuerySelector("a");
                if (link is null)
                    continue;

                var href = link.GetAttribute("href");
                if (string.IsNullOrEmpty(href))
                    continue;

                var name = link.TextContent.Trim().Replace("az ", "");
                var description = cells.Length > 1 ? cells[1].TextContent.Trim() : null;

                var fullUrl = href.StartsWith("http")
                    ? href
                    : $"{CommandBaseUrl}{href.TrimStart('/')}";

                // Ensure view parameter
                if (!fullUrl.Contains("view="))
                {
                    fullUrl += fullUrl.Contains("?") ? "&view=azure-cli-latest" : "?view=azure-cli-latest";
                }

                if (!groups.Any(g => g.Name == name))
                {
                    groups.Add(new CommandGroup
                    {
                        Name = name,
                        Url = fullUrl,
                        Description = description
                    });
                }
            }
        }

        return groups;
    }

    private async Task<List<CliCommandDefinition>> ScrapeCommandGroupAsync(
        CommandGroup group,
        CancellationToken cancellationToken)
    {
        var commands = new List<CliCommandDefinition>();

        try
        {
            var doc = await FetchHtmlAsync(group.Url, cancellationToken);

            // Extract subcommands from the page (limit to avoid timeout)
            var subCommands = ExtractSubCommands(doc, group).Take(MaxSubCommandsPerGroup).ToList();
            Logger.LogDebug("Processing {Count} subcommands for {Group}", subCommands.Count, group.Name);

            foreach (var subCommand in subCommands)
            {
                try
                {
                    var command = await ScrapeCommandPageAsync(subCommand.Url, subCommand.CommandParts, cancellationToken);
                    if (command is not null)
                    {
                        commands.Add(command);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Failed to scrape command {Url}", subCommand.Url);
                }
            }

            // Also parse commands from the current page if it has command details
            var pageCommand = ParseCommandFromPage(doc, group);
            if (pageCommand is not null)
            {
                commands.Add(pageCommand);
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to scrape command group page {Url}", group.Url);
        }

        return commands;
    }

    private List<SubCommand> ExtractSubCommands(IDocument doc, CommandGroup group)
    {
        var subCommands = new List<SubCommand>();

        // Look for command tables or lists
        var tables = doc.QuerySelectorAll("table");
        foreach (var table in tables)
        {
            var rows = table.QuerySelectorAll("tbody tr, tr").Skip(1);
            foreach (var row in rows)
            {
                var cells = row.QuerySelectorAll("td").ToArray();
                if (cells.Length < 1)
                    continue;

                var link = cells[0].QuerySelector("a");
                if (link is null)
                    continue;

                var href = link.GetAttribute("href");
                if (string.IsNullOrEmpty(href))
                    continue;

                var commandText = link.TextContent.Trim();
                var commandParts = ParseCommandParts(commandText);

                if (commandParts.Length == 0)
                    continue;

                var fullUrl = href.StartsWith("http")
                    ? href
                    : $"{CommandBaseUrl}{href.TrimStart('/')}";

                if (!fullUrl.Contains("view="))
                {
                    fullUrl += fullUrl.Contains("?") ? "&view=azure-cli-latest" : "?view=azure-cli-latest";
                }

                subCommands.Add(new SubCommand
                {
                    Url = fullUrl,
                    CommandParts = commandParts
                });
            }
        }

        return subCommands;
    }

    private static string[] ParseCommandParts(string commandText)
    {
        // "az storage account create" -> ["storage", "account", "create"]
        var cleaned = commandText.Trim().ToLowerInvariant();
        if (cleaned.StartsWith("az "))
        {
            cleaned = cleaned[3..];
        }

        return cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }

    private async Task<CliCommandDefinition?> ScrapeCommandPageAsync(
        string url,
        string[] commandParts,
        CancellationToken cancellationToken)
    {
        var doc = await FetchHtmlAsync(url, cancellationToken);
        return ParseCommandFromPage(doc, new CommandGroup
        {
            Name = string.Join(" ", commandParts),
            Url = url,
            Description = null
        });
    }

    private CliCommandDefinition? ParseCommandFromPage(IDocument doc, CommandGroup group)
    {
        var commandParts = ParseCommandParts(group.Name);
        if (commandParts.Length == 0)
            return null;

        // Get description
        var description = ExtractDescription(doc);

        // Parse options
        var options = ExtractOptions(doc, commandParts);

        if (options.Count == 0)
            return null;

        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;
        var className = GenerateClassName(commandParts);

        return new CliCommandDefinition
        {
            FullCommand = $"az {string.Join(" ", commandParts)}",
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = "AzOptions",
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = group.Url,
            Options = options,
            PositionalArguments = [],
            SubDomainGroup = subDomain,
            Enums = options.Where(o => o.EnumDefinition is not null).Select(o => o.EnumDefinition!).ToList()
        };
    }

    private static string? ExtractDescription(IDocument doc)
    {
        var meta = doc.QuerySelector("meta[name='description']");
        if (meta is not null)
        {
            var content = meta.GetAttribute("content");
            if (!string.IsNullOrEmpty(content))
                return content;
        }

        var firstPara = doc.QuerySelector("article p, .content p");
        return firstPara?.TextContent.Trim();
    }

    private List<CliOptionDefinition> ExtractOptions(IDocument doc, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var className = GenerateClassName(commandParts);

        // Azure docs use definition lists or tables for parameters
        var parameterSections = doc.QuerySelectorAll("h2, h3");

        foreach (var section in parameterSections)
        {
            var sectionText = section.TextContent.ToLowerInvariant();
            if (!sectionText.Contains("parameter") && !sectionText.Contains("argument"))
                continue;

            // Get content until next heading
            var sibling = section.NextElementSibling;
            while (sibling is not null && !sibling.TagName.StartsWith("H", StringComparison.OrdinalIgnoreCase))
            {
                // Parse definition lists
                var dts = sibling.QuerySelectorAll("dt");
                var dds = sibling.QuerySelectorAll("dd");

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

                // Parse tables
                if (sibling.TagName.ToUpperInvariant() == "TABLE")
                {
                    var tableOptions = ParseParameterTable(sibling, className);
                    foreach (var opt in tableOptions)
                    {
                        if (!options.Any(o => o.SwitchName == opt.SwitchName))
                        {
                            options.Add(opt);
                        }
                    }
                }

                sibling = sibling.NextElementSibling;
            }
        }

        // Also look for code blocks with usage
        var codeBlocks = doc.QuerySelectorAll("pre code");
        foreach (var block in codeBlocks)
        {
            var matches = OptionPattern().Matches(block.TextContent);
            foreach (Match match in matches)
            {
                var longForm = match.Groups["long"].Value.Trim();
                if (string.IsNullOrEmpty(longForm) || options.Any(o => o.SwitchName == longForm))
                    continue;

                var shortForm = match.Groups["short"].Value.Trim();
                var propertyName = NormalizePropertyName(longForm);
                if (propertyName is null)
                    continue;

                options.Add(new CliOptionDefinition
                {
                    SwitchName = longForm,
                    ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                    PropertyName = propertyName,
                    CSharpType = "string?",
                    Description = null,
                    IsFlag = false,
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

        // Parse option names
        var parts = text.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries);
        string? longForm = null;
        string? shortForm = null;

        foreach (var part in parts)
        {
            var cleaned = part.Trim();
            if (cleaned.StartsWith("--"))
                longForm = cleaned;
            else if (cleaned.StartsWith("-") && cleaned.Length == 2)
                shortForm = cleaned;
        }

        if (string.IsNullOrEmpty(longForm))
            return null;

        var propertyName = NormalizePropertyName(longForm);
        if (propertyName is null)
            return null;

        var isFlag = description?.Contains("flag", StringComparison.OrdinalIgnoreCase) == true ||
                     description?.Contains("boolean", StringComparison.OrdinalIgnoreCase) == true;
        var isNumeric = description?.Contains("integer", StringComparison.OrdinalIgnoreCase) == true;
        var acceptsMultiple = description?.Contains("multiple", StringComparison.OrdinalIgnoreCase) == true ||
                              description?.Contains("list", StringComparison.OrdinalIgnoreCase) == true;

        var enumDef = DetectEnumValues(propertyName, className, null, description);
        var csharpType = DetermineCSharpType(isFlag, acceptsMultiple, false, isNumeric, enumDef);

        return new CliOptionDefinition
        {
            SwitchName = longForm,
            ShortForm = shortForm,
            PropertyName = propertyName,
            CSharpType = csharpType,
            Description = description,
            IsFlag = isFlag,
            IsRequired = description?.Contains("required", StringComparison.OrdinalIgnoreCase) == true,
            AcceptsMultipleValues = acceptsMultiple,
            IsKeyValue = false,
            IsNumeric = isNumeric,
            ValueSeparator = " ",
            EnumDefinition = enumDef
        };
    }

    private List<CliOptionDefinition> ParseParameterTable(IElement table, string className)
    {
        var options = new List<CliOptionDefinition>();

        var rows = table.QuerySelectorAll("tbody tr, tr").Skip(1);
        foreach (var row in rows)
        {
            var cells = row.QuerySelectorAll("td").ToArray();
            if (cells.Length < 2)
                continue;

            var nameCell = cells[0].TextContent.Trim();
            var description = cells.Length > 1 ? cells[1].TextContent.Trim() : null;

            if (!nameCell.StartsWith("-"))
                continue;

            var parts = nameCell.Split([' ', ',', '/'], StringSplitOptions.RemoveEmptyEntries);
            string? longForm = null;
            string? shortForm = null;

            foreach (var part in parts)
            {
                if (part.StartsWith("--"))
                    longForm = part;
                else if (part.StartsWith("-") && part.Length == 2)
                    shortForm = part;
            }

            if (string.IsNullOrEmpty(longForm))
            {
                if (string.IsNullOrEmpty(shortForm))
                    continue;
                longForm = shortForm;
                shortForm = null;
            }

            var propertyName = NormalizePropertyName(longForm);
            if (propertyName is null)
                continue;

            var isFlag = description?.Contains("flag", StringComparison.OrdinalIgnoreCase) == true;
            var isNumeric = description?.Contains("integer", StringComparison.OrdinalIgnoreCase) == true;
            var acceptsMultiple = description?.Contains("multiple", StringComparison.OrdinalIgnoreCase) == true;

            var enumDef = DetectEnumValues(propertyName, className, null, description);
            var csharpType = DetermineCSharpType(isFlag, acceptsMultiple, false, isNumeric, enumDef);

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = description?.Contains("required", StringComparison.OrdinalIgnoreCase) == true,
                AcceptsMultipleValues = acceptsMultiple,
                IsKeyValue = false,
                IsNumeric = isNumeric,
                ValueSeparator = " ",
                EnumDefinition = enumDef
            });
        }

        return options;
    }

    private record CommandGroup
    {
        public required string Name { get; init; }
        public required string Url { get; init; }
        public string? Description { get; init; }
    }

    private record SubCommand
    {
        public required string Url { get; init; }
        public required string[] CommandParts { get; init; }
    }

    [GeneratedRegex(@"(?:(?<short>-\w),?\s*)?(?<long>--[\w-]+)", RegexOptions.Multiline)]
    private static partial Regex OptionPattern();
}
