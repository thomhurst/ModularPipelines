namespace ModularPipelines.Build.Settings;

internal static class FastFailValidation
{
    private const string EnvironmentVariableName = "FAST_FAIL_VALIDATED";

    public static bool IsComplete =>
        string.Equals(
            Environment.GetEnvironmentVariable(EnvironmentVariableName),
            bool.TrueString,
            StringComparison.OrdinalIgnoreCase);
}
