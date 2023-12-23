using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "delete-aggregation-authorization")]
public record AwsConfigserviceDeleteAggregationAuthorizationOptions(
[property: CommandSwitch("--authorized-account-id")] string AuthorizedAccountId,
[property: CommandSwitch("--authorized-aws-region")] string AuthorizedAwsRegion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}