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
        ArgumentNullException.ThrowIfNull(logger);

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
