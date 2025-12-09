using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "update-query-suggestions-config")]
public record AwsKendraUpdateQuerySuggestionsConfigOptions(
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--query-log-look-back-window-in-days")]
    public int? QueryLogLookBackWindowInDays { get; set; }

    [CliOption("--minimum-number-of-querying-users")]
    public int? MinimumNumberOfQueryingUsers { get; set; }

    [CliOption("--minimum-query-count")]
    public int? MinimumQueryCount { get; set; }

    [CliOption("--attribute-suggestions-config")]
    public string? AttributeSuggestionsConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}