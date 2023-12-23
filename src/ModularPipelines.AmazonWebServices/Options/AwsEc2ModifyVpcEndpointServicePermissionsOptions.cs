using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpc-endpoint-service-permissions")]
public record AwsEc2ModifyVpcEndpointServicePermissionsOptions(
[property: CommandSwitch("--service-id")] string ServiceId
) : AwsOptions
{
    [CommandSwitch("--add-allowed-principals")]
    public string[]? AddAllowedPrincipals { get; set; }

    [CommandSwitch("--remove-allowed-principals")]
    public string[]? RemoveAllowedPrincipals { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}