using ModularPipelines.Models;
using TUnit.Assertions;

namespace ModularPipelines.TestHelpers.Assertions;

/// <summary>
/// Common assertion helpers for module results to reduce duplication in tests.
/// </summary>
public static class ModuleResultAssertions
{
    /// <summary>
    /// Asserts that a module result represents a successful execution with a non-null value.
    /// Checks: status is ModuleStatus.Succeeded, exception is null, value is not null.
    /// </summary>
    /// <remarks>
    /// The signature matches the <c>ModuleResult&lt;T&gt;</c> returned by
    /// <see cref="Module{T}.GetAwaiter"/>.
    /// </remarks>
    public static async Task AssertSuccessWithValue<T>(ModuleResult<T> moduleResult)
        where T : class
    {
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Status).IsEqualTo(Enums.ModuleStatus.Succeeded);
            await Assert.That(moduleResult.ExceptionOrDefault).IsNull();
            await Assert.That(moduleResult.ValueOrDefault).IsNotNull();
        }
    }

    /// <summary>
    /// Asserts that a module result represents a successful execution.
    /// Checks: status is ModuleStatus.Succeeded and exception is null.
    /// </summary>
    /// <remarks>
    /// The signature matches the <c>ModuleResult&lt;T&gt;</c> returned by
    /// <see cref="Module{T}.GetAwaiter"/>.
    /// </remarks>
    public static async Task AssertSuccess<T>(ModuleResult<T> moduleResult)
    {
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Status).IsEqualTo(Enums.ModuleStatus.Succeeded);
            await Assert.That(moduleResult.ExceptionOrDefault).IsNull();
        }
    }

    /// <summary>
    /// Asserts that a command result has the expected output with no errors.
    /// Checks: StandardError is null or empty, StandardOutput (trimmed) equals expected value.
    /// </summary>
    /// <param name="moduleResult">The module result containing a CommandResult.</param>
    /// <param name="expectedOutput">The expected standard output value (will be compared after trimming).</param>
    public static async Task AssertCommandOutput(ModuleResult<CommandResult> moduleResult, string expectedOutput)
    {
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.Value.StandardOutput.Trim()).IsEqualTo(expectedOutput);
        }
    }
}
