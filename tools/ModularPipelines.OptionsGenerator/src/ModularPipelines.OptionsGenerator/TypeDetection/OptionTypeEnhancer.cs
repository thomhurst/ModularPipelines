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
            var enhancedOptions = await EnhanceCommandOptionsAsync(
                command,
                toolDefinition.ToolName,
                manualOverridesOnly,
                cancellationToken).ConfigureAwait(false);

            // Option metadata is authoritative after enhancement; retain only unrelated
            // command enums so an old definition cannot shadow its replacement or fallback.
            var originalOptionEnums = command.Options.Where(option => option.EnumDefinition is not null)
                .Select(option => option.EnumDefinition!.EnumName)
                .ToHashSet(StringComparer.Ordinal);
            var allEnums = enhancedOptions.Where(option => option.EnumDefinition is not null)
                .Select(option => option.EnumDefinition!)
                .Concat(command.Enums.Where(enumDefinition => !originalOptionEnums.Contains(enumDefinition.EnumName)))
                .DistinctBy(e => e.EnumName)
                .ToList();

            enhancedCommands.Add(command with { Options = enhancedOptions, Enums = allEnums });
        }

        return toolDefinition with { Commands = enhancedCommands };
    }

    private async Task<List<CliOptionDefinition>> EnhanceCommandOptionsAsync(
        CliCommandDefinition command,
        string toolName,
        bool manualOverridesOnly,
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
                manualOverridesOnly,
                cancellationToken).ConfigureAwait(false);

            enhancedOptions.Add(enhanced);
        }

        return enhancedOptions;
    }

    private async Task<CliOptionDefinition> EnhanceOptionAsync(
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
                ? await _pipeline.DetectManualOverrideAsync(context, cancellationToken).ConfigureAwait(false)
                : await _pipeline.DetectTypeAsync(context, cancellationToken).ConfigureAwait(false);
            detectionResult = result;

            if (result.Type != CliOptionType.Unknown && result.Confidence >= MinimumConfidenceToEnhance)
            {
                // Check if we detected enum values - create an enum definition
                var hasDetectedChoices = result.Type == CliOptionType.Enum && result.EnumValues is { Length: > 0 };
                if (hasDetectedChoices)
                {
                    enumDef = OptionEnumFactory.TryCreate(command.ClassName, option.PropertyName, option.SwitchName,
                        result.EnumValues!.Select(value => (value, option.EnumDefinition?.Values
                            .FirstOrDefault(existing => existing.CliValue.Equals(value, StringComparison.Ordinal))?.Description)));
                }

                // Use existing enum def or newly created one
                var effectiveEnumDef = hasDetectedChoices ? enumDef : option.EnumDefinition;
                var description = hasDetectedChoices && enumDef is null
                    ? OptionEnumFactory.PreserveChoices(option.Description, result.EnumValues!)
                    : option.Description;
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
                    || enumDef is not null
                    || description != option.Description)
                {
                    if (_logger.IsEnabled(LogLevel.Information))
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
                    }

                    enhancedOption = option with
                    {
                        CSharpType = newCSharpType,
                        Description = description,
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

        return ApplySecretMetadata(enhancedOption, command, detectionResult);
    }

    private CliOptionDefinition ApplySecretMetadata(
        CliOptionDefinition option,
        CliCommandDefinition command,
        OptionTypeDetectionResult? detectionResult)
    {
        var secretValueKeys = detectionResult?.SecretValueKeys ?? option.SecretValueKeys;
        var explicitlySecret = detectionResult?.IsSecret;
        var requestsSecret = secretValueKeys.Count > 0
                             || (explicitlySecret ?? IsInferredSecret(option));
        var isBoolean = option.IsFlag
                        || string.Equals(
                            option.CSharpType.TrimEnd('?'),
                            "bool",
                            StringComparison.Ordinal);

        if (isBoolean && requestsSecret)
        {
            _logger.LogWarning(
                "Secret-looking option {Command} {Option} was detected as boolean and cannot be masked",
                command.FullCommand,
                option.SwitchName);
        }

        return option with
        {
            IsSecret = !isBoolean && requestsSecret,
            SecretValueKeys = isBoolean ? [] : secretValueKeys,
        };
    }

    private static bool IsInferredSecret(CliOptionDefinition option)
    {
        // Documented enum choices and resource references do not contain credential material.
        if (option.EnumDefinition is not null || option.IsResourceReference)
        {
            return false;
        }

        var description = option.ValueShapeDescription ?? option.Description;
        return GeneratorUtils.IsSecretOption(option.PropertyName, false, description)
               || (option.IsSecret
                   && !GeneratorUtils.IsFilePathOption(option.PropertyName, description)
                   && !GeneratorUtils.IsSecretMetadataOption(option.PropertyName, description)
                   && !GeneratorUtils.IsResourceIdentifierOption(description));
    }

    /// <summary>
    /// Creates an enhancer with the default pipeline configuration.
    /// </summary>
    public static OptionTypeEnhancer CreateDefault(ILoggerFactory loggerFactory, string? overridesDirectory = null)
    {
        var executor = new ProcessCliCommandExecutor(
            loggerFactory.CreateLogger<ProcessCliCommandExecutor>());

        return CreateDefault(executor, loggerFactory, overridesDirectory);
    }

    internal static OptionTypeEnhancer CreateDefault(
        ICliCommandExecutor executor,
        ILoggerFactory loggerFactory,
        string? overridesDirectory = null)
    {
        // Both default construction paths give enhancement its own retry/circuit state.
        var resilientExecutor = new ResilientCliCommandExecutor(
            executor,
            loggerFactory.CreateLogger<ResilientCliCommandExecutor>());
        var pipeline = OptionTypeDetectorPipeline.CreateDefault(
            resilientExecutor,
            loggerFactory,
            overridesDirectory);

        return new OptionTypeEnhancer(
            pipeline,
            loggerFactory.CreateLogger<OptionTypeEnhancer>());
    }
}
