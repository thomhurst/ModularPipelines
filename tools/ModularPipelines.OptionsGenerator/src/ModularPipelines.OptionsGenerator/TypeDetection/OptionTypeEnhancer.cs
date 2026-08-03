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
    public Task<CliToolDefinition> EnhanceAsync(
        CliToolDefinition toolDefinition,
        CancellationToken cancellationToken = default)
    {
        return EnhanceAsync(
            toolDefinition,
            manualOverridesOnly: false,
            cancellationToken);
    }

    /// <summary>
    /// Applies only per-tool manual overrides. This keeps CLI-first generation
    /// deterministic without re-running help-text and heuristic detection.
    /// </summary>
    public Task<CliToolDefinition> EnhanceManualOverridesAsync(
        CliToolDefinition toolDefinition,
        CancellationToken cancellationToken = default)
    {
        return EnhanceAsync(
            toolDefinition,
            manualOverridesOnly: true,
            cancellationToken);
    }

    private async Task<CliToolDefinition> EnhanceAsync(
        CliToolDefinition toolDefinition,
        bool manualOverridesOnly,
        CancellationToken cancellationToken)
    {
        var enhancedCommands = new List<CliCommandDefinition>();

        foreach (var command in toolDefinition.Commands)
        {
            var (enhancedOptions, detectedEnums) = await EnhanceCommandOptionsAsync(
                command,
                toolDefinition.ToolName,
                manualOverridesOnly,
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
        bool manualOverridesOnly,
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
                manualOverridesOnly,
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
        bool manualOverridesOnly,
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
        var enhancedOption = option;
        CliEnumDefinition? enumDef = null;
        OptionTypeDetectionResult? detectionResult = null;

        try
        {
            var result = manualOverridesOnly
                ? await _pipeline.DetectManualOverrideAsync(context, cancellationToken)
                : await _pipeline.DetectTypeAsync(context, cancellationToken);
            detectionResult = result;

            if (result.Type != CliOptionType.Unknown && result.Confidence >= MinimumConfidenceToEnhance)
            {
                // Check if we detected enum values - create an enum definition
                if (result.Type == CliOptionType.Enum && result.EnumValues is { Length: > 0 })
                {
                    enumDef = CreateEnumDefinition(option, command, result.EnumValues);
                }

                // Use existing enum def or newly created one
                var effectiveEnumDef = enumDef ?? option.EnumDefinition;
                var acceptsMultipleValues = result.Type == CliOptionType.StringList
                    || (result.Type == CliOptionType.Enum
                        && (result.AcceptsMultipleValues || option.AcceptsMultipleValues));
                var detectedCSharpType = CliTypeMapper.ToCSharpType(result.Type, effectiveEnumDef);
                var newCSharpType = result.Type == CliOptionType.Enum && acceptsMultipleValues
                    ? $"IEnumerable<{detectedCSharpType.TrimEnd('?')}>?"
                    : detectedCSharpType;
                var newIsFlag = result.Type == CliOptionType.Bool;

                // Only update if the detection changed the type
                if (newCSharpType != option.CSharpType
                    || newIsFlag != option.IsFlag
                    || result.GroupValues != option.GroupValues
                    || enumDef is not null)
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

                    enhancedOption = option with
                    {
                        CSharpType = newCSharpType,
                        IsFlag = newIsFlag,
                        IsNumeric = result.Type == CliOptionType.Int || result.Type == CliOptionType.Decimal,
                        AcceptsMultipleValues = acceptsMultipleValues,
                        GroupValues = result.GroupValues,
                        IsKeyValue = result.Type == CliOptionType.KeyValue,
                        ValueSeparator = newIsFlag ? " " : option.ValueSeparator,
                        EnumDefinition = effectiveEnumDef
                    };
                }
            }
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            _logger.LogWarning(ex,
                "Failed to enhance type for {Command} {Option}",
                command.FullCommand, option.SwitchName);
        }

        return (ApplySecretMetadata(enhancedOption, command, detectionResult), enumDef);
    }

    private CliOptionDefinition ApplySecretMetadata(
        CliOptionDefinition option,
        CliCommandDefinition command,
        OptionTypeDetectionResult? detectionResult)
    {
        var secretValueKeys = detectionResult?.SecretValueKeys ?? option.SecretValueKeys;
        var hasSecretKeyword = GeneratorUtils.IsSecretOption(
            option.PropertyName,
            isFlag: false,
            option.Description);
        var explicitlySecret = detectionResult?.IsSecret;
        var inferredSecret = (option.IsSecret
                              && !GeneratorUtils.IsFilePathOption(
                                  option.PropertyName,
                                  option.Description))
                             || hasSecretKeyword;
        var requestsSecret = secretValueKeys.Count > 0
                             || (explicitlySecret ?? inferredSecret);

        if (option.IsFlag && requestsSecret)
        {
            _logger.LogWarning(
                "Secret-looking option {Command} {Option} was detected as a flag and cannot be masked",
                command.FullCommand,
                option.SwitchName);
        }

        return option with
        {
            IsSecret = !option.IsFlag && requestsSecret,
            SecretValueKeys = secretValueKeys,
        };
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
