using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "authorize-security-group-ingress")]
public record AwsEc2AuthorizeSecurityGroupIngressOptions : AwsOptions
{
    [CliOption("--group-id")]
    public string? GroupId { get; set; }

    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--ip-permissions")]
    public string[]? IpPermissions { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--cidr")]
    public string? Cidr { get; set; }

    [CliOption("--source-group")]
    public string? SourceGroup { get; set; }

    [CliOption("--group-owner")]
    public string? GroupOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}