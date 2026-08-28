using ModularPipelines.Context;

namespace ModularPipelines.Build.Settings;

internal static class FastFailValidation
{
    private const string EnvironmentVariableName = "FAST_FAIL_VALIDATED";

    public static bool IsComplete(IModuleContext context) =>
        string.Equals(
            context.Environment.Variables.Get(EnvironmentVariableName),
            bool.TrueString,
            StringComparison.OrdinalIgnoreCase);
}
