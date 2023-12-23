using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "create-usage-plan-key")]
public record AwsApigatewayCreateUsagePlanKeyOptions(
[property: CommandSwitch("--usage-plan-id")] string UsagePlanId,
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--key-type")] string KeyType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}