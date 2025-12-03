using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Base;

/// <summary>
/// Base class for all CLI documentation scrapers providing common functionality.
/// </summary>
public abstract partial class CliDocumentationScraperBase : ICliDocumentationScraper
{
    protected readonly HttpClient HttpClient;
    protected readonly ILogger Logger;
    protected readonly IHtmlParser HtmlParser;

    public abstract string ToolName { get; }
    public abstract string NamespacePrefix { get; }
    public abstract string TargetNamespace { get; }
    public abstract string OutputDirectory { get; }

    protected CliDocumentationScraperBase(HttpClient httpClient, ILogger logger)
    {
        HttpClient = httpClient;
        Logger = logger;
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);
        HtmlParser = context.GetService<IHtmlParser>() ?? new HtmlParser();
    }

    public abstract Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetches and parses HTML content from a URL.
    /// </summary>
    protected async Task<IDocument> FetchHtmlAsync(string url, CancellationToken cancellationToken)
    {
        Logger.LogDebug("Fetching {Url}", url);
        var html = await HttpClient.GetStringAsync(url, cancellationToken);
        return await HtmlParser.ParseDocumentAsync(html, cancellationToken);
    }

    /// <summary>
    /// Normalizes CLI option names to C# property names.
    /// </summary>
    protected static string NormalizePropertyName(string optionName)
    {
        // Remove leading dashes
        var cleaned = optionName.TrimStart('-');

        // Split by dash or underscore and convert to PascalCase
        var parts = cleaned.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries);
        return string.Join("", parts.Select(ToPascalCase));
    }

    /// <summary>
    /// Converts a string to PascalCase.
    /// </summary>
    protected static string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpperInvariant(input[0]) + input[1..].ToLowerInvariant();
    }

    /// <summary>
    /// Generates a class name from command parts.
    /// </summary>
    protected string GenerateClassName(string[] commandParts)
    {
        var pascalParts = commandParts.Select(ToPascalCase);
        return $"{NamespacePrefix}{string.Join("", pascalParts)}Options";
    }

    /// <summary>
    /// Determines the C# type from CLI option metadata.
    /// </summary>
    protected static string DetermineCSharpType(
        bool isFlag,
        bool acceptsMultipleValues,
        bool isKeyValue,
        bool isNumeric,
        CliEnumDefinition? enumDefinition)
    {
        if (isFlag) return "bool?";
        if (enumDefinition is not null) return $"{enumDefinition.EnumName}?";
        if (isKeyValue) return "IEnumerable<KeyValue>?";
        if (acceptsMultipleValues) return "string[]?";
        if (isNumeric) return "int?";
        return "string?";
    }

    /// <summary>
    /// Parses a description to detect if this is a boolean flag.
    /// </summary>
    protected static bool DetectBooleanFlag(string? description, string? valueType)
    {
        if (string.IsNullOrEmpty(valueType))
            return true; // No value type means it's likely a flag

        var lowerType = valueType.ToLowerInvariant();
        return lowerType is "bool" or "boolean" or "" or "flag";
    }

    /// <summary>
    /// Parses a description to detect if this option accepts multiple values.
    /// </summary>
    protected static bool DetectMultipleValues(string? description, string? valueType)
    {
        if (string.IsNullOrEmpty(valueType) && string.IsNullOrEmpty(description))
            return false;

        var combined = $"{valueType} {description}".ToLowerInvariant();
        return combined.Contains("multiple") ||
               combined.Contains("can be specified more than once") ||
               combined.Contains("may be specified multiple") ||
               combined.Contains("[]") ||
               combined.Contains("list of") ||
               combined.Contains("comma-separated");
    }

    /// <summary>
    /// Parses a description to detect if this is a numeric option.
    /// </summary>
    protected static bool DetectNumericType(string? valueType)
    {
        if (string.IsNullOrEmpty(valueType))
            return false;

        var lowerType = valueType.ToLowerInvariant();
        return lowerType is "int" or "integer" or "number" or "int32" or "int64" or "uint";
    }

    /// <summary>
    /// Parses a value type string to detect enum values (e.g., "json|yaml|table").
    /// </summary>
    protected static CliEnumDefinition? DetectEnumValues(string propertyName, string commandClassName, string? valueType, string? description)
    {
        if (string.IsNullOrEmpty(valueType))
            return null;

        // Look for patterns like: json|yaml|table or {json, yaml, table}
        var pipeMatch = EnumPipePattern().Match(valueType);
        if (pipeMatch.Success)
        {
            var values = valueType.Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrEmpty(v) && v.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_'))
                .ToList();

            if (values.Count >= 2)
            {
                return CreateEnumDefinition(propertyName, commandClassName, values);
            }
        }

        // Look for brace patterns like {Public, Private}
        var braceMatch = EnumBracePattern().Match(valueType);
        if (braceMatch.Success)
        {
            var innerContent = braceMatch.Groups[1].Value;
            var values = innerContent.Split([',', ' '], StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();

            if (values.Count >= 2)
            {
                return CreateEnumDefinition(propertyName, commandClassName, values);
            }
        }

        return null;
    }

    private static CliEnumDefinition CreateEnumDefinition(string propertyName, string commandClassName, List<string> values)
    {
        // Generate command-specific enum name
        var enumName = $"{commandClassName.Replace("Options", "")}{propertyName}";

        return new CliEnumDefinition
        {
            EnumName = enumName,
            Values = values.Select(v => new CliEnumValue
            {
                MemberName = NormalizeEnumMemberName(v),
                CliValue = v
            }).ToList()
        };
    }

    private static string NormalizeEnumMemberName(string value)
    {
        // Convert to PascalCase, handle special characters
        var cleaned = value.Replace("-", "_").Replace(".", "_");
        var parts = cleaned.Split('_', StringSplitOptions.RemoveEmptyEntries);
        return string.Join("", parts.Select(ToPascalCase));
    }

    /// <summary>
    /// Escapes text for XML documentation comments.
    /// </summary>
    protected static string EscapeXmlComment(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\r\n", " ")
            .Replace("\n", " ")
            .Replace("\r", " ")
            .Trim();
    }

    [GeneratedRegex(@"^[\w-]+\|[\w-]+")]
    private static partial Regex EnumPipePattern();

    [GeneratedRegex(@"\{([^}]+)\}")]
    private static partial Regex EnumBracePattern();
}
