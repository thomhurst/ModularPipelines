using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "wait", "key-pair-exists")]
public record AwsEc2WaitKeyPairExistsOptions : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--key-names")]
    public string[]? KeyNames { get; set; }

    [CliOption("--key-pair-ids")]
    public string[]? KeyPairIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}