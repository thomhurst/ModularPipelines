using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpc-endpoint-service-permissions")]
public record AwsEc2ModifyVpcEndpointServicePermissionsOptions(
[property: CliOption("--service-id")] string ServiceId
) : AwsOptions
{
    [CliOption("--add-allowed-principals")]
    public string[]? AddAllowedPrincipals { get; set; }

    [CliOption("--remove-allowed-principals")]
    public string[]? RemoveAllowedPrincipals { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}