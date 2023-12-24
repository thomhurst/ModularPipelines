using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "get-usage")]
public record AwsApigatewayGetUsageOptions(
[property: CommandSwitch("--usage-plan-id")] string UsagePlanId,
[property: CommandSwitch("--start-date")] string StartDate,
[property: CommandSwitch("--end-date")] string EndDate
) : AwsOptions
{
    [CommandSwitch("--key-id")]
    public string? KeyId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}