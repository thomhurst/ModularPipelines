using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-security-group")]
public record AwsEc2CreateSecurityGroupOptions(
[property: CliOption("--description")] string Description,
[property: CliOption("--group-name")] string GroupName
) : AwsOptions
{
    [CliOption("--vpc-id")]
    public string? VpcId { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}