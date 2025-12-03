using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Highest priority detector that uses manual override files.
/// Override files are JSON with explicit type definitions for specific options.
/// </summary>
public class ManualOverrideDetector : IOptionTypeDetector
{
    private readonly ILogger<ManualOverrideDetector> _logger;
    private readonly string _overridesDirectory;
    private readonly Dictionary<string, ToolOverrides> _cache = new();
    private static readonly object CacheLock = new();

    public int Priority => 0; // Highest priority
    public string Name => "ManualOverride";

    public ManualOverrideDetector(ILogger<ManualOverrideDetector> logger, string? overridesDirectory = null)
    {
        _logger = logger;
        _overridesDirectory = overridesDirectory ?? GetDefaultOverridesDirectory();
    }

    public bool CanHandle(string toolName)
    {
        // Check if we have an override file for this tool
        var overrideFile = GetOverrideFilePath(toolName);
        return File.Exists(overrideFile);
    }

    public Task<OptionTypeDetectionResult> DetectTypeAsync(
        OptionDetectionContext context,
        CancellationToken cancellationToken = default)
    {
        var overrides = LoadOverrides(context.ToolName);
        if (overrides is null)
        {
            return Task.FromResult(OptionTypeDetectionResult.Unknown(Name));
        }

        // Build the command key (e.g., "container.run" for "docker container run")
        var commandKey = string.Join(".", context.CommandPath.Skip(1));

        // Check for command-specific override first
        if (overrides.Commands.TryGetValue(commandKey, out var commandOverrides))
        {
            foreach (var optionName in context.AllNames)
            {
                if (commandOverrides.TryGetValue(optionName, out var optionOverride))
                {
                    return Task.FromResult(CreateResult(optionOverride, $"Command-specific: {commandKey}"));
                }
            }
        }

        // Check for global tool-level override
        foreach (var optionName in context.AllNames)
        {
            if (overrides.Global.TryGetValue(optionName, out var optionOverride))
            {
                return Task.FromResult(CreateResult(optionOverride, "Global override"));
            }
        }

        return Task.FromResult(OptionTypeDetectionResult.Unknown(Name));
    }

    private OptionTypeDetectionResult CreateResult(OptionOverride optionOverride, string scope)
    {
        _logger.LogDebug("Using manual override: {Type} ({Scope})", optionOverride.Type, scope);

        return new OptionTypeDetectionResult
        {
            Type = optionOverride.Type,
            Confidence = 100,
            Source = Name,
            Notes = $"{scope}: {optionOverride.Reason ?? "Manual override"}"
        };
    }

    private ToolOverrides? LoadOverrides(string toolName)
    {
        lock (CacheLock)
        {
            if (_cache.TryGetValue(toolName, out var cached))
            {
                return cached;
            }

            var filePath = GetOverrideFilePath(toolName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            try
            {
                var json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new CliOptionTypeConverter() }
                };

                var overrides = JsonSerializer.Deserialize<ToolOverrides>(json, options);
                if (overrides is not null)
                {
                    _cache[toolName] = overrides;
                }

                return overrides;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load override file: {Path}", filePath);
                return null;
            }
        }
    }

    private string GetOverrideFilePath(string toolName)
    {
        return Path.Combine(_overridesDirectory, $"{toolName.ToLowerInvariant()}.json");
    }

    private static string GetDefaultOverridesDirectory()
    {
        // Look for overrides directory relative to the assembly
        var assemblyDir = AppContext.BaseDirectory;
        var overridesDir = Path.Combine(assemblyDir, "TypeOverrides");

        if (Directory.Exists(overridesDir))
            return overridesDir;

        // Try relative to project directory (for development)
        var projectDir = Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", ".."));
        var devOverridesDir = Path.Combine(projectDir, "TypeOverrides");

        return Directory.Exists(devOverridesDir) ? devOverridesDir : overridesDir;
    }
}

/// <summary>
/// JSON structure for tool override files.
/// </summary>
public class ToolOverrides
{
    /// <summary>
    /// Global overrides that apply to all commands for this tool.
    /// Key is the option name (e.g., "--verbose").
    /// </summary>
    public Dictionary<string, OptionOverride> Global { get; set; } = new();

    /// <summary>
    /// Command-specific overrides.
    /// Key is the command path (e.g., "container.run").
    /// Value is a dictionary of option overrides.
    /// </summary>
    public Dictionary<string, Dictionary<string, OptionOverride>> Commands { get; set; } = new();
}

/// <summary>
/// Override definition for a single option.
/// </summary>
public class OptionOverride
{
    /// <summary>
    /// The correct type for this option.
    /// </summary>
    public CliOptionType Type { get; set; }

    /// <summary>
    /// Optional reason for the override (for documentation).
    /// </summary>
    public string? Reason { get; set; }
}

/// <summary>
/// JSON converter for CliOptionType enum.
/// </summary>
public class CliOptionTypeConverter : System.Text.Json.Serialization.JsonConverter<CliOptionType>
{
    public override CliOptionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value?.ToLowerInvariant() switch
        {
            "bool" or "boolean" => CliOptionType.Bool,
            "string" => CliOptionType.String,
            "int" or "integer" => CliOptionType.Int,
            "decimal" or "float" or "double" => CliOptionType.Decimal,
            "stringlist" or "list" or "array" => CliOptionType.StringList,
            "keyvalue" or "map" or "dictionary" => CliOptionType.KeyValue,
            _ => CliOptionType.Unknown
        };
    }

    public override void Write(Utf8JsonWriter writer, CliOptionType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
