namespace ModularPipelines.UnitTests;

/// <summary>
/// Shared test constants used across multiple test files to eliminate magic strings.
/// </summary>
public static class TestConstants
{
    /// <summary>
    /// A common test string used for file content, command output, and general test data.
    /// </summary>
    public const string TestString = "Foo bar!";

    /// <summary>
    /// An error message prefix used in requirement failure tests.
    /// </summary>
    public const string ErrorPrefix = "Error: ";

    /// <summary>
    /// The full error message used in requirement failure tests.
    /// </summary>
    public const string RequirementErrorMessage = ErrorPrefix + TestString;
}
