using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers;

internal static class AzCliCompatibility
{
    private static readonly IReadOnlyList<CliCompatibilityMethod> CreateOrUpdateMethods =
    [
        new()
        {
            MethodName = "Create_or_update",
            ObsoleteMessage = "Use CreateOrUpdate instead.",
        },
    ];

    private static readonly IReadOnlyList<CliCompatibilityMethod> UpsertScopeMethods =
    [
        new()
        {
            MethodName = "Upsert_scope",
            ObsoleteMessage = "Use UpsertScope instead.",
        },
    ];

    public static IReadOnlyList<CliCompatibilityMethod> GetMethods(IReadOnlyList<string> commandParts)
    {
        return string.Join(" ", commandParts) switch
        {
            "security automation create_or_update" => CreateOrUpdateMethods,
            "security alerts-suppression-rule upsert_scope" => UpsertScopeMethods,
            _ => [],
        };
    }
}
