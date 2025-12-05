using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Base;

/// <summary>
/// Base class for all CLI documentation scrapers providing common functionality.
/// Scrapers focus on HTML parsing; type enhancement is applied as post-processing
/// via <see cref="OptionTypeEnhancer"/>.
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
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

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
    /// Returns null if the option name is invalid (e.g., just dashes, contains special characters).
    /// </summary>
    protected static string? NormalizePropertyName(string optionName)
    {
        // Skip if the switch name contains special characters (like examples with quotes/equals)
        // e.g., --security-opt="label=user:USER" is an example, not a valid option
        if (optionName.Contains('=') || optionName.Contains('"') || optionName.Contains('\'') || optionName.Contains(':'))
            return null;

        // Remove leading dashes
        var cleaned = optionName.TrimStart('-');

        // Skip if there's nothing left after removing dashes
        if (string.IsNullOrWhiteSpace(cleaned))
            return null;

        // Skip if it's just a line of dashes (table separator)
        if (cleaned.All(c => c == '-' || c == '_'))
            return null;

        // Split by dash or underscore and convert to PascalCase
        var parts = cleaned.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries);

        // Skip if no valid parts remain
        if (parts.Length == 0)
            return null;

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
        // Handle hyphens within command parts (e.g., "build-server" -> "BuildServer")
        var expandedParts = commandParts
            .SelectMany(part => part.Split('-', StringSplitOptions.RemoveEmptyEntries))
            .Select(ToPascalCase);
        return $"{NamespacePrefix}{string.Join("", expandedParts)}Options";
    }

    /// <summary>
    /// Determines the C# type from CLI option metadata.
    /// For more accurate type detection, use <see cref="OptionTypeEnhancer"/> as post-processing.
    /// </summary>
    protected static string DetermineCSharpType(
        bool isFlag,
        bool acceptsMultipleValues,
        bool isKeyValue,
        bool isNumeric,
        CliEnumDefinition? enumDefinition)
    {
        if (isFlag) return CliTypeMapper.ToCSharpType(CliOptionType.Bool);
        if (enumDefinition is not null) return CliTypeMapper.ToCSharpType(CliOptionType.Enum, enumDefinition);
        if (isKeyValue) return CliTypeMapper.ToCSharpType(CliOptionType.KeyValue);
        if (acceptsMultipleValues) return CliTypeMapper.ToCSharpType(CliOptionType.StringList);
        if (isNumeric) return CliTypeMapper.ToCSharpType(CliOptionType.Int);
        return CliTypeMapper.ToCSharpType(CliOptionType.String);
    }

    /// <summary>
    /// Parses option metadata to detect if this is a boolean flag (no value required).
    /// Uses reliable signals from documentation:
    /// - Default value is exactly "true" or "false"
    /// - Accepted values contain boolean patterns like {0, 1, f, false, n, no, t, true, y, yes}
    /// - Explicit type annotation like "bool" or "boolean"
    /// </summary>
    protected static bool DetectBooleanFlag(string? description, string? defaultValue, string? acceptedValues = null, string? explicitType = null)
    {
        // 1. Explicit type annotation (most reliable)
        if (!string.IsNullOrEmpty(explicitType))
        {
            var lowerType = explicitType.ToLowerInvariant().Trim();
            if (lowerType is "bool" or "boolean" or "flag")
                return true;
            // Explicit non-boolean types
            if (lowerType is "string" or "int" or "integer" or "number" or "path" or "file"
                or "list" or "array" or "duration" or "bytes" or "uint" or "float" or "double")
                return false;
        }

        // 2. Default value is exactly "true" or "false" (kubectl pattern)
        if (!string.IsNullOrEmpty(defaultValue))
        {
            var lowerDefault = defaultValue.ToLowerInvariant().Trim();
            if (lowerDefault is "true" or "false")
                return true;
        }

        // 3. Accepted values contain boolean patterns (Azure CLI pattern)
        if (!string.IsNullOrEmpty(acceptedValues))
        {
            var lowerAccepted = acceptedValues.ToLowerInvariant();
            // Azure CLI pattern: {0, 1, f, false, n, no, t, true, y, yes}
            if ((lowerAccepted.Contains("true") && lowerAccepted.Contains("false")) ||
                (lowerAccepted.Contains("yes") && lowerAccepted.Contains("no")))
                return true;
        }

        // 4. Default: assume it's NOT a boolean (safer to require explicit signals)
        return false;
    }

    /// <summary>
    /// Overload for backwards compatibility with scrapers that don't have acceptedValues/explicitType.
    /// </summary>
    protected static bool DetectBooleanFlag(string? description, string? defaultValue)
    {
        return DetectBooleanFlag(description, defaultValue, null, null);
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
