using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "put-multi-region-access-point-policy")]
public record AwsS3controlPutMultiRegionAccessPointPolicyOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--details")] string Details
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}