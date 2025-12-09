using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "associate-security-key")]
public record AwsConnectAssociateSecurityKeyOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--key")] string Key
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}