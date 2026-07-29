namespace ModularPipelines.Console;

internal static class OutputLoggerCategories
{
    public const string Pipeline = "ModularPipelines.Output";

    public static string ForModule(Type moduleType) =>
        moduleType == typeof(void)
            ? Pipeline
            : moduleType.FullName ?? moduleType.Name;
}
