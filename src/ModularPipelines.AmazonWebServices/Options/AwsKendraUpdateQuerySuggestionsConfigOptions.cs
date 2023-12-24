using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "update-query-suggestions-config")]
public record AwsKendraUpdateQuerySuggestionsConfigOptions(
[property: CommandSwitch("--index-id")] string IndexId
) : AwsOptions
{
    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--query-log-look-back-window-in-days")]
    public int? QueryLogLookBackWindowInDays { get; set; }

    [CommandSwitch("--minimum-number-of-querying-users")]
    public int? MinimumNumberOfQueryingUsers { get; set; }

    [CommandSwitch("--minimum-query-count")]
    public int? MinimumQueryCount { get; set; }

    [CommandSwitch("--attribute-suggestions-config")]
    public string? AttributeSuggestionsConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}