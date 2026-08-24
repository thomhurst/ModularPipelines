using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Runs a module when an environment variable is set or equals an expected value.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfEnvironmentVariableAttribute : RunIfAllAttribute, IPlanningConditionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RunIfEnvironmentVariableAttribute"/> class.
    /// </summary>
    /// <param name="variableName">The environment variable name.</param>
    /// <param name="expectedValue">
    /// Optional required value. When omitted, any set value, including an empty value, satisfies the condition.
    /// </param>
    public RunIfEnvironmentVariableAttribute(string variableName, string? expectedValue = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(variableName);
        VariableName = variableName;
        ExpectedValue = expectedValue;
    }

    /// <summary>
    /// Gets the environment variable name.
    /// </summary>
    public string VariableName { get; }

    /// <summary>
    /// Gets the optional required value.
    /// </summary>
    public string? ExpectedValue { get; }

    /// <inheritdoc />
    public override string ConditionNames => $"RunIfEnvironmentVariable({VariableName})";

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        Task.FromResult(EnvironmentVariableCondition.Matches(context, VariableName, ExpectedValue));
}

/// <summary>
/// Skips a module when an environment variable is set or equals an expected value.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfEnvironmentVariableAttribute : SkipIfAttribute, IPlanningConditionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkipIfEnvironmentVariableAttribute"/> class.
    /// </summary>
    /// <param name="variableName">The environment variable name.</param>
    /// <param name="expectedValue">
    /// Optional required value. When omitted, any set value, including an empty value, satisfies the condition.
    /// </param>
    public SkipIfEnvironmentVariableAttribute(string variableName, string? expectedValue = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(variableName);
        VariableName = variableName;
        ExpectedValue = expectedValue;
    }

    /// <summary>
    /// Gets the environment variable name.
    /// </summary>
    public string VariableName { get; }

    /// <summary>
    /// Gets the optional required value.
    /// </summary>
    public string? ExpectedValue { get; }

    /// <inheritdoc />
    public override string ConditionNames => $"SkipIfEnvironmentVariable({VariableName})";

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        Task.FromResult(EnvironmentVariableCondition.Matches(context, VariableName, ExpectedValue));
}

/// <summary>
/// Runs a module when an environment variable is not set.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfEnvironmentVariableUnsetAttribute : RunIfAllAttribute, IPlanningConditionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RunIfEnvironmentVariableUnsetAttribute"/> class.
    /// </summary>
    /// <param name="variableName">The environment variable name.</param>
    public RunIfEnvironmentVariableUnsetAttribute(string variableName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(variableName);
        VariableName = variableName;
    }

    /// <summary>
    /// Gets the environment variable name.
    /// </summary>
    public string VariableName { get; }

    /// <inheritdoc />
    public override string ConditionNames => $"RunIfEnvironmentVariableUnset({VariableName})";

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        Task.FromResult(!EnvironmentVariableCondition.IsSet(context, VariableName));
}

/// <summary>
/// Skips a module when an environment variable is not set.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfEnvironmentVariableUnsetAttribute : SkipIfAttribute, IPlanningConditionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkipIfEnvironmentVariableUnsetAttribute"/> class.
    /// </summary>
    /// <param name="variableName">The environment variable name.</param>
    public SkipIfEnvironmentVariableUnsetAttribute(string variableName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(variableName);
        VariableName = variableName;
    }

    /// <summary>
    /// Gets the environment variable name.
    /// </summary>
    public string VariableName { get; }

    /// <inheritdoc />
    public override string ConditionNames => $"SkipIfEnvironmentVariableUnset({VariableName})";

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        Task.FromResult(!EnvironmentVariableCondition.IsSet(context, VariableName));
}

internal static class EnvironmentVariableCondition
{
    public static bool Matches(IPipelineContext context, string variableName, string? expectedValue)
    {
        var actualValue = GetValue(context, variableName);
        return expectedValue is null
            ? actualValue is not null
            : string.Equals(actualValue, expectedValue, StringComparison.Ordinal);
    }

    public static bool IsSet(IPipelineContext context, string variableName) =>
        GetValue(context, variableName) is not null;

    private static string? GetValue(IPipelineContext context, string variableName) =>
        context.Environment.Variables.GetEnvironmentVariable(variableName);
}
