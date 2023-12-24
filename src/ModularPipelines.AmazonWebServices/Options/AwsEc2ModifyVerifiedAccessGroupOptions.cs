using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-verified-access-group")]
public record AwsEc2ModifyVerifiedAccessGroupOptions(
[property: CommandSwitch("--verified-access-group-id")] string VerifiedAccessGroupId
) : AwsOptions
{
    [CommandSwitch("--verified-access-instance-id")]
    public string? VerifiedAccessInstanceId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}