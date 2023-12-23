using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "release-address")]
public record AwsEc2ReleaseAddressOptions : AwsOptions
{
    [CommandSwitch("--allocation-id")]
    public string? AllocationId { get; set; }

    [CommandSwitch("--public-ip")]
    public string? PublicIp { get; set; }

    [CommandSwitch("--network-border-group")]
    public string? NetworkBorderGroup { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}