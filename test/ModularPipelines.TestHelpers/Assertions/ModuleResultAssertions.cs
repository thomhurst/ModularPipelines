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
    /// Checks: ModuleResultType is Success, Exception is null, Value is not null.
    /// </summary>
    public static async Task AssertSuccessWithValue<T>(ModuleResult<T> moduleResult)
        where T : class
    {
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    /// <summary>
    /// Asserts that a module result represents a successful execution.
    /// Checks: ModuleResultType is Success, Exception is null.
    /// </summary>
    public static async Task AssertSuccess<T>(ModuleResult<T> moduleResult)
    {
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
        }
    }

    /// <summary>
    /// Asserts that a command result has the expected output with no errors.
    /// Checks: StandardError is null or empty, StandardOutput (trimmed) equals expected value.
    /// </summary>
    /// <param name="moduleResult">The module result containing a CommandResult.</param>
    /// <param name="expectedOutput">The expected standard output value (will be compared after trimming).</param>
    public static async Task AssertCommandOutput(ModuleResult<CommandResult?> moduleResult, string expectedOutput)
    {
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.Value.StandardOutput.Trim()).IsEqualTo(expectedOutput);
        }
    }
}
