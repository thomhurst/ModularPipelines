using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-certificate-provider")]
public record AwsIotUpdateCertificateProviderOptions(
[property: CliOption("--certificate-provider-name")] string CertificateProviderName
) : AwsOptions
{
    [CliOption("--lambda-function-arn")]
    public string? LambdaFunctionArn { get; set; }

    [CliOption("--account-default-for-operations")]
    public string[]? AccountDefaultForOperations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}