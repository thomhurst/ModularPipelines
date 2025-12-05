using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Type detector for Azure CLI.
/// Azure CLI provides structured help output via `az help-dump` and shows
/// "Accepted values: 0, 1, f, false, n, no, t, true, y, yes" for booleans.
/// </summary>
public partial class AzureCliHelpTypeDetector : HelpTextTypeDetectorBase
{
    private const string HelpDumpCacheKey = "AzHelpDump";

    public override int Priority => 100;
    public override string Name => "AzureCliHelp";

    public AzureCliHelpTypeDetector(ICliCommandExecutor executor, ILogger<AzureCliHelpTypeDetector> logger)
        : base(executor, logger)
    {
    }

    public override bool CanHandle(string toolName)
    {
        return toolName.Equals("az", StringComparison.OrdinalIgnoreCase);
    }

    protected override async Task<string?> FetchHelpTextAsync(
        OptionDetectionContext context,
        CancellationToken cancellationToken)
    {
        // Try `az help-dump` first for structured JSON
        var dumpResult = await TryGetHelpDumpAsync(context, cancellationToken);
        if (dumpResult is not null)
        {
            return dumpResult;
        }

        // Fall back to regular --help
        var arguments = string.Join(" ", context.CommandPath.Skip(1)) + " --help";
        var result = await Executor.ExecuteAsync("az", arguments.Trim(), cancellationToken);

        return result.Success ? result.CombinedOutput : null;
    }

    private async Task<string?> TryGetHelpDumpAsync(
        OptionDetectionContext context,
        CancellationToken cancellationToken)
    {
        var cacheKey = (HelpDumpCacheKey, string.Join("_", context.CommandPath));

        if (context.CommandCache.TryGetValue(cacheKey, out var cached) && cached is string cachedDump)
        {
            return cachedDump;
        }

        // Try to get structured help
        var arguments = string.Join(" ", context.CommandPath.Skip(1)) + " --help --output json";
        var result = await Executor.ExecuteAsync("az", arguments.Trim(), cancellationToken);

        if (result.Success && result.StandardOutput.TrimStart().StartsWith('{'))
        {
            context.CommandCache[cacheKey] = result.StandardOutput;
            return result.StandardOutput;
        }

        return null;
    }

    protected override OptionTypeDetectionResult ParseOptionFromHelpText(
        string helpText,
        OptionDetectionContext context)
    {
        // Try to parse as JSON first (from help-dump)
        if (helpText.TrimStart().StartsWith('{'))
        {
            var jsonResult = TryParseFromJson(helpText, context);
            if (jsonResult.Type != CliOptionType.Unknown)
            {
                return jsonResult;
            }
        }

        // Fall back to text-based parsing
        return ParseFromTextHelp(helpText, context);
    }

    private OptionTypeDetectionResult TryParseFromJson(string json, OptionDetectionContext context)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Navigate to parameters
            if (root.TryGetProperty("parameters", out var parameters))
            {
                foreach (var param in parameters.EnumerateArray())
                {
                    var name = param.TryGetProperty("name", out var nameProp)
                        ? nameProp.GetString()
                        : null;

                    if (name is null || !context.AllNames.Contains(name))
                        continue;

                    // Check for type information
                    if (param.TryGetProperty("type", out var typeProp))
                    {
                        var typeStr = typeProp.GetString()?.ToLowerInvariant();
                        var detectedType = typeStr switch
                        {
                            "bool" or "boolean" => CliOptionType.Bool,
                            "int" or "integer" => CliOptionType.Int,
                            "float" or "decimal" => CliOptionType.Decimal,
                            "list" or "array" => CliOptionType.StringList,
                            _ => CliOptionType.String
                        };

                        return new OptionTypeDetectionResult
                        {
                            Type = detectedType,
                            Confidence = 98,
                            Source = Name,
                            Notes = $"From Azure CLI JSON help: type={typeStr}"
                        };
                    }
                }
            }
        }
        catch (JsonException)
        {
            // Invalid JSON, fall through to text parsing
        }

        return OptionTypeDetectionResult.Unknown(Name);
    }

    private OptionTypeDetectionResult ParseFromTextHelp(string helpText, OptionDetectionContext context)
    {
        foreach (var optionName in context.AllNames)
        {
            var optionLine = FindAzureOptionLine(helpText, optionName);
            if (optionLine is null)
                continue;

            // Check for accepted values pattern
            var acceptedMatch = AcceptedValuesPattern().Match(optionLine);
            if (acceptedMatch.Success)
            {
                var acceptedValues = acceptedMatch.Groups["values"].Value.ToLowerInvariant();

                // Check for boolean pattern
                if (IsBooleanAcceptedValues(acceptedValues))
                {
                    Logger.LogDebug("Detected {Option} as Bool (accepted values indicate boolean)", optionName);
                    return new OptionTypeDetectionResult
                    {
                        Type = CliOptionType.Bool,
                        Confidence = 95,
                        Source = Name,
                        Notes = $"Boolean accepted values: {acceptedValues}"
                    };
                }

                // Otherwise it's likely an enum, treat as string
                Logger.LogDebug("Detected {Option} as String (enum values: {Values})", optionName, acceptedValues);
                return new OptionTypeDetectionResult
                {
                    Type = CliOptionType.String,
                    Confidence = 90,
                    Source = Name,
                    Notes = $"Enum values: {acceptedValues}"
                };
            }

            // Check for Required: True/False pattern
            if (optionLine.Contains("Required:", StringComparison.OrdinalIgnoreCase))
            {
                // Azure CLI options with no accepted values are typically strings
                Logger.LogDebug("Detected {Option} as String (no accepted values, has Required field)", optionName);
                return new OptionTypeDetectionResult
                {
                    Type = CliOptionType.String,
                    Confidence = 70,
                    Source = Name,
                    Notes = "Default to string - has Required field but no accepted values"
                };
            }
        }

        return OptionTypeDetectionResult.Unknown(Name);
    }

    private static string? FindAzureOptionLine(string helpText, string optionName)
    {
        // Azure CLI help often spans multiple lines per option
        // Look for the option and capture surrounding context
        var lines = helpText.Split('\n');
        var escapedName = Regex.Escape(optionName);

        for (var i = 0; i < lines.Length; i++)
        {
            if (Regex.IsMatch(lines[i], $@"(^|\s){escapedName}(\s|$)", RegexOptions.IgnoreCase))
            {
                // Capture this line and a few following lines for context
                var contextLines = lines.Skip(i).Take(5);
                return string.Join("\n", contextLines);
            }
        }

        return null;
    }

    private static bool IsBooleanAcceptedValues(string acceptedValues)
    {
        // Azure CLI shows: "Accepted values: 0, 1, f, false, n, no, t, true, y, yes"
        var boolIndicators = new[] { "true", "false", "yes", "no" };
        return boolIndicators.All(indicator =>
            acceptedValues.Contains(indicator, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Matches "Accepted values: value1, value2, ..." pattern.
    /// </summary>
    [GeneratedRegex(@"Accepted\s+values?:\s*(?<values>[^\n\r]+)", RegexOptions.IgnoreCase)]
    private static partial Regex AcceptedValuesPattern();
}
