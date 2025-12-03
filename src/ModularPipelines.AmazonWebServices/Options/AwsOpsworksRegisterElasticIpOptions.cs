using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "register-elastic-ip")]
public record AwsOpsworksRegisterElasticIpOptions(
[property: CliOption("--elastic-ip")] string ElasticIp,
[property: CliOption("--stack-id")] string StackId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}