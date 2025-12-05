using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudhsmv2", "delete-hsm")]
public record AwsCloudhsmv2DeleteHsmOptions(
[property: CliOption("--cluster-id")] string ClusterId
) : AwsOptions
{
    [CliOption("--hsm-id")]
    public string? HsmId { get; set; }

    [CliOption("--eni-id")]
    public string? EniId { get; set; }

    [CliOption("--eni-ip")]
    public string? EniIp { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}