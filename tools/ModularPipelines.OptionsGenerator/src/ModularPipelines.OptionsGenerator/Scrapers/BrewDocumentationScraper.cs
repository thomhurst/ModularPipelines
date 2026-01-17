using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;

namespace ModularPipelines.OptionsGenerator.Scrapers;

/// <summary>
/// Scrapes Homebrew CLI documentation from docs.brew.sh/Manpage.
/// The manpage lists all commands with their options in a structured format.
/// </summary>
public partial class BrewDocumentationScraper : CliDocumentationScraperBase
{
    private const string ManpageUrl = "https://docs.brew.sh/Manpage";

    public override string ToolName => "brew";
    public override string NamespacePrefix => "Brew";
    public override string TargetNamespace => "ModularPipelines.Homebrew";
    public override string OutputDirectory => "src/ModularPipelines.Homebrew";

    // Commands to skip (meta commands that don't make sense as pipeline operations)
    private static readonly HashSet<string> SkipCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "help", "commands", "shellenv", "analytics", "completions"
    };

    public BrewDocumentationScraper(HttpClient httpClient, ILogger<BrewDocumentationScraper> logger)
        : base(httpClient, logger)
    {
    }

    public override async Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default)
    {
        var commands = new List<CliCommandDefinition>();
        var errors = new List<ScrapingError>();

        Logger.LogInformation("Fetching Homebrew manpage from {Url}", ManpageUrl);

        try
        {
            var doc = await FetchHtmlAsync(ManpageUrl, cancellationToken);

            // Find all command headers (h3 elements with command names in code/backticks)
            var headers = doc.QuerySelectorAll("h3");

            foreach (var header in headers)
            {
                try
                {
                    var command = ParseCommandFromHeader(header, doc);
                    if (command is not null)
                    {
                        commands.Add(command);
                        Logger.LogDebug("Scraped command: {Command}", command.FullCommand);
                    }
                }
                catch (Exception ex)
                {
                    var headerText = header.TextContent.Trim();
                    Logger.LogWarning(ex, "Failed to parse command from header: {Header}", headerText);
                    errors.Add(new ScrapingError
                    {
                        Source = $"header: {headerText}",
                        Message = ex.Message,
                        Exception = ex
                    });
                }
            }

            Logger.LogInformation("Scraped {Count} commands from Homebrew manpage", commands.Count);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to fetch Homebrew manpage");
            errors.Add(new ScrapingError
            {
                Source = ManpageUrl,
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

    private CliCommandDefinition? ParseCommandFromHeader(IElement header, IDocument doc)
    {
        var headerText = header.TextContent.Trim();

        // Extract command name from header (format: "command-name [options] [arguments]")
        var commandMatch = CommandHeaderPattern().Match(headerText);
        if (!commandMatch.Success)
        {
            return null;
        }

        var commandName = commandMatch.Groups["command"].Value.Trim();

        // Skip utility commands
        if (SkipCommands.Contains(commandName))
        {
            return null;
        }

        // Handle subcommands (e.g., "brew install" vs "brew cask install")
        var commandParts = commandName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Get the description and options from content following the header
        var (description, options) = ParseCommandContent(header, commandParts);

        var className = GenerateClassName(commandParts);

        return new CliCommandDefinition
        {
            FullCommand = $"brew {commandName}",
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = "BrewOptions",
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = $"{ManpageUrl}#{commandName.Replace(" ", "-")}",
            Options = options,
            PositionalArguments = [],
            SubDomainGroup = null,
            Enums = options.Where(o => o.EnumDefinition is not null).Select(o => o.EnumDefinition!).ToList()
        };
    }

    private (string? Description, List<CliOptionDefinition> Options) ParseCommandContent(IElement header, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        string? description = null;
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var className = GenerateClassName(commandParts);

        // Get all sibling elements until the next h3 or h2
        var current = header.NextElementSibling;
        var descriptionParts = new List<string>();
        var inDescription = true;

        while (current is not null && current.TagName.ToUpperInvariant() is not "H2" and not "H3")
        {
            var tagName = current.TagName.ToUpperInvariant();
            var text = current.TextContent.Trim();

            if (tagName == "P")
            {
                if (inDescription && !text.StartsWith("-") && !text.StartsWith("`-"))
                {
                    descriptionParts.Add(text);
                }
                else
                {
                    inDescription = false;
                    // Check if this paragraph contains an option
                    var optionMatch = OptionInParagraphPattern().Match(text);
                    if (optionMatch.Success)
                    {
                        var option = ParseOptionFromText(text, className, seenOptions);
                        if (option is not null)
                        {
                            options.Add(option);
                        }
                    }
                }
            }
            else if (tagName == "UL" || tagName == "DL")
            {
                inDescription = false;
                // Parse options from list items
                var items = current.QuerySelectorAll("li, dt, dd, p, code");
                foreach (var item in items)
                {
                    var itemText = item.TextContent.Trim();
                    if (itemText.StartsWith("-") || itemText.StartsWith("`-"))
                    {
                        var option = ParseOptionFromText(itemText, className, seenOptions);
                        if (option is not null)
                        {
                            options.Add(option);
                        }
                    }
                }
            }
            else if (tagName == "PRE" || tagName == "CODE")
            {
                inDescription = false;
                // Check for options in code blocks
                var lines = text.Split('\n');
                foreach (var line in lines)
                {
                    if (line.TrimStart().StartsWith("-"))
                    {
                        var option = ParseOptionFromText(line.Trim(), className, seenOptions);
                        if (option is not null)
                        {
                            options.Add(option);
                        }
                    }
                }
            }

            current = current.NextElementSibling;
        }

        if (descriptionParts.Count > 0)
        {
            description = string.Join(" ", descriptionParts);
            // Truncate if too long
            if (description.Length > 500)
            {
                description = description[..497] + "...";
            }
        }

        return (description, options);
    }

    private CliOptionDefinition? ParseOptionFromText(string text, string className, HashSet<string> seenOptions)
    {
        // Clean up the text (remove backticks, leading/trailing whitespace)
        text = text.Replace("`", "").Trim();

        // Parse option patterns:
        // "-d, --debug" or "--formula, --formulae" or "--[no-]quarantine" or "--output=FORMAT"
        var match = OptionPattern().Match(text);
        if (!match.Success)
        {
            return null;
        }

        var flagsPart = match.Groups["flags"].Value.Trim();
        var descriptionPart = match.Groups["description"].Value.Trim();

        // Parse flags
        var flags = flagsPart.Split(',', StringSplitOptions.TrimEntries)
            .Where(f => !string.IsNullOrEmpty(f))
            .ToList();

        // Find long form and short form
        string? longForm = null;
        string? shortForm = null;

        foreach (var flag in flags)
        {
            var cleanFlag = flag.Split('=')[0].Split(' ')[0].Trim();

            if (cleanFlag.Contains("[no-]"))
            {
                // Handle [no-] options
                longForm = cleanFlag.Replace("[no-]", "");
            }
            else if (cleanFlag.StartsWith("--"))
            {
                longForm ??= cleanFlag;
            }
            else if (cleanFlag.StartsWith("-") && cleanFlag.Length == 2)
            {
                shortForm = cleanFlag;
            }
        }

        if (string.IsNullOrEmpty(longForm))
        {
            if (!string.IsNullOrEmpty(shortForm))
            {
                longForm = shortForm;
                shortForm = null;
            }
            else
            {
                return null;
            }
        }

        // Skip duplicates
        if (seenOptions.Contains(longForm))
        {
            return null;
        }

        seenOptions.Add(longForm);

        var propertyName = NormalizePropertyName(longForm);
        if (propertyName is null)
        {
            return null;
        }

        // Determine if it's a flag or takes a value
        var isFlag = !flagsPart.Contains('=') &&
                     !descriptionPart.Contains("=") &&
                     !ValueIndicatorPattern().IsMatch(descriptionPart);

        var csharpType = isFlag ? "bool?" : "string?";

        return new CliOptionDefinition
        {
            SwitchName = longForm,
            ShortForm = shortForm,
            PropertyName = propertyName,
            CSharpType = csharpType,
            Description = string.IsNullOrEmpty(descriptionPart) ? null : descriptionPart,
            IsFlag = isFlag,
            IsRequired = false,
            AcceptsMultipleValues = false,
            IsKeyValue = false,
            IsNumeric = false,
            ValueSeparator = isFlag ? " " : "=",
            EnumDefinition = null,
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
        };
    }

    /// <summary>
    /// Matches command name from header text.
    /// Examples: "install [options] formula", "search [options] text|/regex/"
    /// </summary>
    [GeneratedRegex(@"^(?<command>[\w][\w-]*(?:\s+[\w][\w-]*)*)(?:\s+\[|\s+<|\s+[A-Z]|\s*$)", RegexOptions.IgnoreCase)]
    private static partial Regex CommandHeaderPattern();

    /// <summary>
    /// Matches option definitions.
    /// Examples: "-d, --debug", "--formula", "--output=FORMAT"
    /// </summary>
    [GeneratedRegex(@"^(?<flags>(?:-\w,\s*)?(?:--[\w\[\]-]+(?:=\w+)?(?:,\s*--[\w-]+)*))\s*(?<description>.*)$")]
    private static partial Regex OptionPattern();

    /// <summary>
    /// Matches options embedded in paragraph text.
    /// </summary>
    [GeneratedRegex(@"^-\w|^--[\w-]+")]
    private static partial Regex OptionInParagraphPattern();

    /// <summary>
    /// Matches text that indicates an option takes a value.
    /// </summary>
    [GeneratedRegex(@"\b(?:set|specify|use|path|file|directory|number|name|value|format|type)\b", RegexOptions.IgnoreCase)]
    private static partial Regex ValueIndicatorPattern();
}
