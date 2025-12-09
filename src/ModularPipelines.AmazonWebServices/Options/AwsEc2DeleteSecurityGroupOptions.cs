using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-security-group")]
public record AwsEc2DeleteSecurityGroupOptions : AwsOptions
{
    [CliOption("--group-id")]
    public string? GroupId { get; set; }

    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}