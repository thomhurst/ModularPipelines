using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "release-address")]
public record AwsEc2ReleaseAddressOptions : AwsOptions
{
    [CliOption("--allocation-id")]
    public string? AllocationId { get; set; }

    [CliOption("--public-ip")]
    public string? PublicIp { get; set; }

    [CliOption("--network-border-group")]
    public string? NetworkBorderGroup { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}