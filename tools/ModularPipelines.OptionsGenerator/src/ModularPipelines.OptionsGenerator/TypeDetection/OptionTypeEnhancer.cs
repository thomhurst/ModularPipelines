using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Post-processing service that enhances scraped CLI options with better type detection.
/// This allows scrapers to remain simple while still benefiting from advanced type inference.
/// </summary>
public class OptionTypeEnhancer
{
    /// <summary>
    /// Minimum confidence level required to apply type enhancement.
    /// </summary>
    private const int MinimumConfidenceToEnhance = 70;

    private readonly OptionTypeDetectorPipeline _pipeline;
    private readonly ILogger<OptionTypeEnhancer> _logger;

    public OptionTypeEnhancer(OptionTypeDetectorPipeline pipeline, ILogger<OptionTypeEnhancer> logger)
    {
        ArgumentNullException.ThrowIfNull(pipeline);
        ArgumentNullException.ThrowIfNull(logger);

        _pipeline = pipeline;
        _logger = logger;
    }

    /// <summary>
    /// Enhances all options in a tool definition with better type detection.
    /// </summary>
    public async Task<CliToolDefinition> EnhanceAsync(
        CliToolDefinition toolDefinition,
        CancellationToken cancellationToken = default)
    {
        var enhancedCommands = new List<CliCommandDefinition>();

        foreach (var command in toolDefinition.Commands)
        {
            var enhancedOptions = await EnhanceCommandOptionsAsync(
                command,
                toolDefinition.ToolName,
                cancellationToken);

            enhancedCommands.Add(command with { Options = enhancedOptions });
        }

        return toolDefinition with { Commands = enhancedCommands };
    }

    private async Task<List<CliOptionDefinition>> EnhanceCommandOptionsAsync(
        CliCommandDefinition command,
        string toolName,
        CancellationToken cancellationToken)
    {
        var enhancedOptions = new List<CliOptionDefinition>();
        var commandCache = new Dictionary<object, object>();

        foreach (var option in command.Options)
        {
            var enhanced = await EnhanceOptionAsync(
                option,
                command,
                toolName,
                commandCache,
                cancellationToken);

            enhancedOptions.Add(enhanced);
        }

        return enhancedOptions;
    }

    private async Task<CliOptionDefinition> EnhanceOptionAsync(
        CliOptionDefinition option,
        CliCommandDefinition command,
        string toolName,
        IDictionary<object, object> commandCache,
        CancellationToken cancellationToken)
    {
        // Build the list of all names for this option
        var allNames = new List<string> { option.SwitchName };
        if (!string.IsNullOrEmpty(option.ShortForm))
        {
            allNames.Add(option.ShortForm);
        }

        // Create detection context with shared cache for efficiency
        var context = new OptionDetectionContext
        {
            OptionName = option.SwitchName,
            AllNames = allNames,
            ToolName = toolName,
            CommandPath = [toolName, .. command.CommandParts],
            Description = option.Description,
            DefaultValue = null, // Would need to be extracted separately
            AcceptedValues = null,
            CommandCache = commandCache // Share cache directly - no copying needed
        };

        try
        {
            var result = await _pipeline.DetectTypeAsync(context, cancellationToken);

            if (result.Type != CliOptionType.Unknown && result.Confidence >= MinimumConfidenceToEnhance)
            {
                var newCSharpType = CliTypeMapper.ToCSharpType(result.Type, option.EnumDefinition);
                var newIsFlag = result.Type == CliOptionType.Bool;

                // Only update if the detection changed the type
                if (newCSharpType != option.CSharpType || newIsFlag != option.IsFlag)
                {
                    _logger.LogInformation(
                        "Enhanced {Command} {Option}: {OldType} -> {NewType} (confidence: {Confidence}, source: {Source})",
                        command.FullCommand,
                        option.SwitchName,
                        option.CSharpType,
                        newCSharpType,
                        result.Confidence,
                        result.Source);

                    return option with
                    {
                        CSharpType = newCSharpType,
                        IsFlag = newIsFlag,
                        IsNumeric = result.Type == CliOptionType.Int || result.Type == CliOptionType.Decimal,
                        AcceptsMultipleValues = result.Type == CliOptionType.StringList,
                        IsKeyValue = result.Type == CliOptionType.KeyValue,
                        ValueSeparator = newIsFlag ? " " : "="
                    };
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Failed to enhance type for {Command} {Option}",
                command.FullCommand, option.SwitchName);
        }

        return option;
    }

    /// <summary>
    /// Creates an enhancer with the default pipeline configuration.
    /// </summary>
    public static OptionTypeEnhancer CreateDefault(ILoggerFactory loggerFactory, string? overridesDirectory = null)
    {
        var executor = new ProcessCliCommandExecutor(
            loggerFactory.CreateLogger<ProcessCliCommandExecutor>());

        var pipeline = OptionTypeDetectorPipeline.CreateDefault(
            executor,
            loggerFactory,
            overridesDirectory);

        return new OptionTypeEnhancer(
            pipeline,
            loggerFactory.CreateLogger<OptionTypeEnhancer>());
    }
}
