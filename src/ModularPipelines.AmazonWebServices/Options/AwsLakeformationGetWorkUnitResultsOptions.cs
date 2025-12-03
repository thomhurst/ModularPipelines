using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "get-work-unit-results")]
public record AwsLakeformationGetWorkUnitResultsOptions(
[property: CliOption("--query-id")] string QueryId,
[property: CliOption("--work-unit-id")] long WorkUnitId,
[property: CliOption("--work-unit-token")] string WorkUnitToken
) : AwsOptions;