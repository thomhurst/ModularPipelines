using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-workforce")]
public record AwsSagemakerUpdateWorkforceOptions(
[property: CliOption("--workforce-name")] string WorkforceName
) : AwsOptions
{
    [CliOption("--source-ip-config")]
    public string? SourceIpConfig { get; set; }

    [CliOption("--oidc-config")]
    public string? OidcConfig { get; set; }

    [CliOption("--workforce-vpc-config")]
    public string? WorkforceVpcConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}