using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
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
            var (enhancedOptions, detectedEnums) = await EnhanceCommandOptionsAsync(
                command,
                toolDefinition.ToolName,
                cancellationToken);

            // Merge existing enums with newly detected enums
            var allEnums = command.Enums.Concat(detectedEnums)
                .DistinctBy(e => e.EnumName)
                .ToList();

            enhancedCommands.Add(command with { Options = enhancedOptions, Enums = allEnums });
        }

        return toolDefinition with { Commands = enhancedCommands };
    }

    private async Task<(List<CliOptionDefinition> Options, List<CliEnumDefinition> Enums)> EnhanceCommandOptionsAsync(
        CliCommandDefinition command,
        string toolName,
        CancellationToken cancellationToken)
    {
        var enhancedOptions = new List<CliOptionDefinition>();
        var detectedEnums = new List<CliEnumDefinition>();
        var commandCache = new Dictionary<object, object>();

        foreach (var option in command.Options)
        {
            var (enhanced, enumDef) = await EnhanceOptionAsync(
                option,
                command,
                toolName,
                commandCache,
                cancellationToken);

            enhancedOptions.Add(enhanced);

            if (enumDef is not null)
            {
                detectedEnums.Add(enumDef);
            }
        }

        return (enhancedOptions, detectedEnums);
    }

    private async Task<(CliOptionDefinition Option, CliEnumDefinition? EnumDef)> EnhanceOptionAsync(
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
                // Check if we detected enum values - create an enum definition
                CliEnumDefinition? enumDef = null;
                if (result.Type == CliOptionType.Enum && result.EnumValues is { Length: > 0 })
                {
                    enumDef = CreateEnumDefinition(option, command, result.EnumValues);
                }

                // Use existing enum def or newly created one
                var effectiveEnumDef = enumDef ?? option.EnumDefinition;
                var newCSharpType = CliTypeMapper.ToCSharpType(result.Type, effectiveEnumDef);
                var newIsFlag = result.Type == CliOptionType.Bool;

                // Only update if the detection changed the type
                if (newCSharpType != option.CSharpType || newIsFlag != option.IsFlag || enumDef is not null)
                {
                    _logger.LogInformation(
                        "Enhanced {Command} {Option}: {OldType} -> {NewType} (confidence: {Confidence}, source: {Source}){EnumInfo}",
                        command.FullCommand,
                        option.SwitchName,
                        option.CSharpType,
                        newCSharpType,
                        result.Confidence,
                        result.Source,
                        enumDef is not null ? $" [Enum: {enumDef.EnumName}]" : "");

                    var enhancedOption = option with
                    {
                        CSharpType = newCSharpType,
                        IsFlag = newIsFlag,
                        IsNumeric = result.Type == CliOptionType.Int || result.Type == CliOptionType.Decimal,
                        AcceptsMultipleValues = result.Type == CliOptionType.StringList,
                        IsKeyValue = result.Type == CliOptionType.KeyValue,
                        ValueSeparator = newIsFlag ? " " : "=",
                        EnumDefinition = effectiveEnumDef
                    };

                    return (enhancedOption, enumDef);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Failed to enhance type for {Command} {Option}",
                command.FullCommand, option.SwitchName);
        }

        return (option, null);
    }

    /// <summary>
    /// Creates an enum definition from detected enum values.
    /// </summary>
    private static CliEnumDefinition CreateEnumDefinition(
        CliOptionDefinition option,
        CliCommandDefinition command,
        string[] enumValues)
    {
        // Generate enum name based on command and option
        // e.g., "DotNetBuildVerbosity" for dotnet build --verbosity
        var commandPrefix = command.ClassName.Replace("Options", "");
        var enumName = GeneratorUtils.ToEnumName(option.SwitchName, commandPrefix);

        // Create enum values
        var values = enumValues
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(cliValue => new CliEnumValue
            {
                MemberName = GeneratorUtils.ToEnumMemberName(cliValue),
                CliValue = cliValue,
                Description = null
            })
            .DistinctBy(v => v.MemberName) // Avoid duplicate member names
            .ToList();

        return new CliEnumDefinition
        {
            EnumName = enumName,
            Values = values,
            Description = $"Allowed values for the {option.SwitchName} option."
        };
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
