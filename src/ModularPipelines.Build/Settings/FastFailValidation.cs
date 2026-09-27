using ModularPipelines.Context;

namespace ModularPipelines.Build.Settings;

internal static class FastFailValidation
{
    private const string EnvironmentVariableName = "FAST_FAIL_VALIDATED";

    public static bool IsComplete(IModuleContext context) =>
        IsValidated(context.Environment.Variables.Get(EnvironmentVariableName));

    public static bool IsComplete(Func<string, string?> getEnvironmentVariable) =>
        IsValidated(getEnvironmentVariable(EnvironmentVariableName));

    private static bool IsValidated(string? value) =>
        string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase);
}
