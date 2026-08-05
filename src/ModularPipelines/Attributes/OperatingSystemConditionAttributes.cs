using System.Runtime.InteropServices;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Runs a module on any of the selected operating systems.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfOperatingSystemAttribute : RunIfAllAttribute, IOperatingSystemConditionAttribute, IPlanningConditionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RunIfOperatingSystemAttribute"/> class.
    /// </summary>
    /// <param name="operatingSystems">The operating systems on which the module may run.</param>
    public RunIfOperatingSystemAttribute(params OperatingSystemIdentifier[] operatingSystems)
    {
        OperatingSystems = OperatingSystemCondition.Normalize(operatingSystems);
    }

    /// <summary>
    /// Gets the operating systems on which the module may run.
    /// </summary>
    public IReadOnlyList<OperatingSystemIdentifier> OperatingSystems { get; }

    /// <inheritdoc />
    public override string ConditionNames =>
        $"RunIfOperatingSystem({string.Join(", ", OperatingSystems)})";

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        Task.FromResult(OperatingSystemCondition.Matches(context, OperatingSystems));
}

/// <summary>
/// Skips a module on any of the selected operating systems.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfOperatingSystemAttribute : SkipIfAttribute, IOperatingSystemConditionAttribute, IPlanningConditionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkipIfOperatingSystemAttribute"/> class.
    /// </summary>
    /// <param name="operatingSystems">The operating systems on which the module is skipped.</param>
    public SkipIfOperatingSystemAttribute(params OperatingSystemIdentifier[] operatingSystems)
    {
        OperatingSystems = OperatingSystemCondition.Normalize(operatingSystems);
    }

    /// <summary>
    /// Gets the operating systems on which the module is skipped.
    /// </summary>
    public IReadOnlyList<OperatingSystemIdentifier> OperatingSystems { get; }

    /// <inheritdoc />
    public override string ConditionNames =>
        $"SkipIfOperatingSystem({string.Join(", ", OperatingSystems)})";

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        Task.FromResult(OperatingSystemCondition.Matches(context, OperatingSystems));
}

internal interface IOperatingSystemConditionAttribute
{
    IReadOnlyList<OperatingSystemIdentifier> OperatingSystems { get; }
}

internal static class OperatingSystemCondition
{
    public static IReadOnlyList<OperatingSystemIdentifier> Normalize(
        OperatingSystemIdentifier[] operatingSystems)
    {
        ArgumentNullException.ThrowIfNull(operatingSystems);
        if (operatingSystems.Length == 0)
        {
            throw new ArgumentException("At least one operating system is required.", nameof(operatingSystems));
        }

        if (operatingSystems.Any(operatingSystem => !Enum.IsDefined(operatingSystem)))
        {
            throw new ArgumentOutOfRangeException(
                nameof(operatingSystems),
                "Every operating system must be a defined OperatingSystemIdentifier value.");
        }

        return operatingSystems.Distinct().ToArray();
    }

    public static bool Matches(
        IPipelineContext context,
        IReadOnlyList<OperatingSystemIdentifier> operatingSystems)
    {
        var currentOperatingSystem = GetIdentifier(context.Environment.OperatingSystem);
        return operatingSystems.Contains(currentOperatingSystem);
    }

    private static OperatingSystemIdentifier GetIdentifier(OSPlatform operatingSystem)
    {
        if (operatingSystem == OSPlatform.Windows)
        {
            return OperatingSystemIdentifier.Windows;
        }

        if (operatingSystem == OSPlatform.Linux)
        {
            return OperatingSystemIdentifier.Linux;
        }

        if (operatingSystem == OSPlatform.OSX)
        {
            return OperatingSystemIdentifier.MacOS;
        }

        return operatingSystem == OSPlatform.FreeBSD
            ? OperatingSystemIdentifier.FreeBSD
            : OperatingSystemIdentifier.Unknown;
    }
}
