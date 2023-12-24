using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "put-access-grants-instance-resource-policy")]
public record AwsS3controlPutAccessGrantsInstanceResourcePolicyOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--policy")] string Policy
) : AwsOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}