using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "get-work-unit-results")]
public record AwsLakeformationGetWorkUnitResultsOptions(
[property: CommandSwitch("--query-id")] string QueryId,
[property: CommandSwitch("--work-unit-id")] long WorkUnitId,
[property: CommandSwitch("--work-unit-token")] string WorkUnitToken
) : AwsOptions;