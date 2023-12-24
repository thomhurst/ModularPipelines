using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "assign-tape-pool")]
public record AwsStoragegatewayAssignTapePoolOptions(
[property: CommandSwitch("--tape-arn")] string TapeArn,
[property: CommandSwitch("--pool-id")] string PoolId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}