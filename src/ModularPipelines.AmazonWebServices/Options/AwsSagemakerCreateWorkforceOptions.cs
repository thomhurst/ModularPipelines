using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-workforce")]
public record AwsSagemakerCreateWorkforceOptions(
[property: CliOption("--workforce-name")] string WorkforceName
) : AwsOptions
{
    [CliOption("--cognito-config")]
    public string? CognitoConfig { get; set; }

    [CliOption("--oidc-config")]
    public string? OidcConfig { get; set; }

    [CliOption("--source-ip-config")]
    public string? SourceIpConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--workforce-vpc-config")]
    public string? WorkforceVpcConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}