using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "list-executors")]
public record AwsAthenaListExecutorsOptions(
[property: CliOption("--session-id")] string SessionId
) : AwsOptions
{
    [CliOption("--executor-state-filter")]
    public string? ExecutorStateFilter { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}