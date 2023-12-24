using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "put-access-point-policy")]
public record AwsS3controlPutAccessPointPolicyOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy")] string Policy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}