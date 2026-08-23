namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Represents a parsed CLI command with its options and metadata.
/// </summary>
public record CliCommandDefinition
{
    /// <summary>
    /// Full command path (e.g., "kubectl config view", "docker buildx build").
    /// </summary>
    public required string FullCommand { get; init; }

    /// <summary>
    /// Command path parts for CommandPrecedingArguments attribute.
    /// </summary>
    public required string[] CommandParts { get; init; }

    /// <summary>
    /// Generated class name (e.g., "KubernetesConfigViewOptions").
    /// </summary>
    public required string ClassName { get; init; }

    /// <summary>
    /// Parent class name (e.g., "KubernetesOptions").
    /// </summary>
    public required string ParentClassName { get; init; }

    /// <summary>
    /// The tool namespace prefix (e.g., "Kubernetes", "Docker").
    /// </summary>
    public required string ToolNamespacePrefix { get; init; }

    /// <summary>
    /// Description for XML documentation.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Documentation URL for reference.
    /// </summary>
    public string? DocumentationUrl { get; init; }

    /// <summary>
    /// All options/flags for this command.
    /// </summary>
    public required IReadOnlyList<CliOptionDefinition> Options { get; init; }

    /// <summary>
    /// Nested argument-group declarations retained from CLI help.
    /// </summary>
    public IReadOnlyList<CliArgumentGroup> ArgumentGroups { get; init; } = [];

    /// <summary>
    /// Positional arguments for this command.
    /// </summary>
    public IReadOnlyList<CliPositionalArgument> PositionalArguments { get; init; } = [];

    /// <summary>
    /// Usage synopsis from which positional arguments were parsed.
    /// </summary>
    public string? UsageSynopsis { get; init; }

    /// <summary>
    /// Whether the usage synopsis declares one or more positional operands.
    /// </summary>
    public bool HasOperandTakingUsage { get; init; }

    /// <summary>
    /// Required options that should be constructor parameters.
    /// </summary>
    public IReadOnlyList<CliOptionDefinition> RequiredOptions =>
        Options.Where(o => o.IsRequired).ToList();

    /// <summary>
    /// The sub-domain group this command belongs to (e.g., "Container", "Image" for Docker).
    /// </summary>
    public string? SubDomainGroup { get; init; }

    /// <summary>
    /// Optional generated identifier for the command's first segment when a tool-specific
    /// compound name cannot be inferred from the CLI spelling.
    /// </summary>
    public string? CommandGroupIdentifierOverride { get; init; }

    /// <summary>
    /// Enums that need to be generated for this command's options.
    /// </summary>
    public IReadOnlyList<CliEnumDefinition> Enums { get; init; } = [];

    /// <summary>
    /// Public properties retained for source and binary compatibility but excluded from CLI rendering.
    /// </summary>
    public IReadOnlyList<CliCompatibilityProperty> CompatibilityProperties { get; init; } = [];

    /// <summary>
    /// Previously generated secondary constructors retained for source compatibility.
    /// </summary>
    public IReadOnlyList<CliCompatibilityConstructor> CompatibilityConstructors { get; init; } = [];

    /// <summary>
    /// Secondary constructors retained on command-group alias option records, keyed by alias class name.
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyList<CliCompatibilityConstructor>> AliasCompatibilityConstructors
    { get; init; } = new Dictionary<string, IReadOnlyList<CliCompatibilityConstructor>>(StringComparer.Ordinal);

    /// <summary>
    /// Enum-valued properties retained on command-group alias records, keyed by alias class name.
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyList<CliAliasCompatibilityProperty>> AliasCompatibilityProperties
    { get; init; } = new Dictionary<string, IReadOnlyList<CliAliasCompatibilityProperty>>(StringComparer.Ordinal);

    /// <summary>
    /// Whether an existing command-group <c>ExecuteAsync</c> facade must remain generated.
    /// </summary>
    public bool PreserveExecuteFacade { get; init; }

    /// <summary>
    /// Whether an existing named command facade must remain when the command gains children.
    /// </summary>
    public bool PreserveNamedFacade { get; init; }

    /// <summary>
    /// Whether an existing facade allowed its options argument to be omitted.
    /// </summary>
    public bool PreserveOptionalOptionsParameter { get; init; }

    /// <summary>
    /// Public methods retained as obsolete forwarding aliases for compatibility.
    /// </summary>
    public IReadOnlyList<CliCompatibilityMethod> CompatibilityMethods { get; init; } = [];

    /// <summary>
    /// Whether this command has been explicitly reviewed as safe for a runnable documentation example.
    /// Commands are unsafe by default; scraped names and descriptions are not enough to establish safety.
    /// </summary>
    public bool IsSafeForDocumentation { get; init; }

    /// <summary>
    /// Whether executing this command can wait for interactive input.
    /// </summary>
    public bool IsInteractive { get; init; }

    /// <summary>
    /// Whether executing this command can modify or delete external state.
    /// </summary>
    public bool IsDestructive { get; init; }

    /// <summary>
    /// C# expressions keyed by generated property name for a complete documentation example.
    /// </summary>
    public IReadOnlyDictionary<string, string> DocumentationExampleValues { get; init; } =
        new Dictionary<string, string>(StringComparer.Ordinal);

    /// <summary>
    /// Verifies that operand-taking usage cannot silently generate an unusable API.
    /// </summary>
    public void ValidateOperandCoverage() =>
        ValidateOperandCoverage(HasOperandTakingUsage, UsageSynopsis);

    /// <summary>
    /// Verifies operand coverage against usage parsed by the shared traversal.
    /// </summary>
    /// <param name="hasOperandTakingUsage">Whether the shared usage parser found operands.</param>
    /// <param name="usageSynopsis">The usage synopsis inspected by the shared parser.</param>
    /// <param name="usagePositionalArguments">Operands parsed from the inspected synopsis.</param>
    public void ValidateOperandCoverage(
        bool hasOperandTakingUsage,
        string? usageSynopsis,
        IReadOnlyList<CliPositionalArgument>? usagePositionalArguments = null)
    {
        if (!hasOperandTakingUsage
            || PositionalArguments.Count > 0
            || AreUsageOperandsCoveredByOptions(usagePositionalArguments))
        {
            return;
        }

        throw new InvalidOperationException(
            $"{FullCommand} declares positional operands in usage '{usageSynopsis}', " +
            "but no CliPositionalArgument values were generated.");
    }

    private bool AreUsageOperandsCoveredByOptions(
        IReadOnlyList<CliPositionalArgument>? usagePositionalArguments) =>
        usagePositionalArguments is { Count: > 0 }
        && usagePositionalArguments.All(argument => Options.Any(option =>
            option.PropertyName.Equals(argument.PropertyName, StringComparison.OrdinalIgnoreCase)
            && argument.AssociatedOptionSwitch is not null
            && (option.SwitchName.Equals(argument.AssociatedOptionSwitch, StringComparison.OrdinalIgnoreCase)
                || option.ShortForm?.Equals(
                    argument.AssociatedOptionSwitch,
                    StringComparison.OrdinalIgnoreCase) == true)));
}

/// <summary>
/// Describes an obsolete generated property retained for API compatibility.
/// </summary>
public record CliCompatibilityProperty
{
    /// <summary>
    /// CLR property name.
    /// </summary>
    public required string PropertyName { get; init; }

    /// <summary>
    /// C# property type.
    /// </summary>
    public required string CSharpType { get; init; }

    /// <summary>
    /// Optional replacement property that this compatibility alias forwards to.
    /// </summary>
    public string? ForwardToPropertyName { get; init; }

    /// <summary>
    /// Whether the forwarding accessor must use <c>init</c> because its target is init-only.
    /// </summary>
    public bool UseInitAccessor { get; init; }

    /// <summary>
    /// Obsolete diagnostic shown to consumers.
    /// </summary>
    public required string ObsoleteMessage { get; init; }
}

/// <summary>
/// Describes a secondary generated constructor retained for source compatibility.
/// </summary>
public record CliCompatibilityConstructor
{
    /// <summary>
    /// Parameters exposed by the retained constructor.
    /// </summary>
    public required IReadOnlyList<CliCompatibilityConstructorParameter> Parameters { get; init; }

    /// <summary>
    /// Arguments forwarded to the current primary constructor.
    /// </summary>
    public required IReadOnlyList<string> PrimaryConstructorArguments { get; init; }

    /// <summary>
    /// Whether to retain the deconstruction shape synthesized by the former primary constructor.
    /// </summary>
    public bool PreserveDeconstruct { get; init; }
}

/// <summary>
/// Describes a parameter on a retained generated constructor.
/// </summary>
public readonly record struct CliCompatibilityConstructorParameter(string PropertyName, string CSharpType);

/// <summary>
/// Describes a property retained on a command-group alias record.
/// </summary>
public record CliAliasCompatibilityProperty
{
    /// <summary>
    /// CLR property name shared with the canonical options record.
    /// </summary>
    public required string PropertyName { get; init; }

    /// <summary>
    /// Previously generated alias type exposed to consumers.
    /// </summary>
    public required string AliasCSharpType { get; init; }

    /// <summary>
    /// Canonical type used by the base options record.
    /// </summary>
    public required string CanonicalCSharpType { get; init; }

    /// <summary>
    /// Whether the retained property uses an init accessor instead of a setter.
    /// </summary>
    public bool UseInitAccessor { get; init; }

    /// <summary>
    /// Obsolete diagnostic shown to consumers.
    /// </summary>
    public required string ObsoleteMessage { get; init; }
}

/// <summary>
/// Describes an obsolete generated method retained as a forwarding alias.
/// </summary>
public record CliCompatibilityMethod
{
    /// <summary>
    /// CLR method name.
    /// </summary>
    public required string MethodName { get; init; }

    /// <summary>
    /// Obsolete diagnostic shown to consumers.
    /// </summary>
    public required string ObsoleteMessage { get; init; }
}
