using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-verified-access-group")]
public record AwsEc2ModifyVerifiedAccessGroupOptions(
[property: CliOption("--verified-access-group-id")] string VerifiedAccessGroupId
) : AwsOptions
{
    [CliOption("--verified-access-instance-id")]
    public string? VerifiedAccessInstanceId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}