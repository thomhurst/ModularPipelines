using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "put-aggregation-authorization")]
public record AwsConfigservicePutAggregationAuthorizationOptions(
[property: CommandSwitch("--authorized-account-id")] string AuthorizedAccountId,
[property: CommandSwitch("--authorized-aws-region")] string AuthorizedAwsRegion
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}