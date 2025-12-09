using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-certificate-provider")]
public record AwsIotCreateCertificateProviderOptions(
[property: CliOption("--certificate-provider-name")] string CertificateProviderName,
[property: CliOption("--lambda-function-arn")] string LambdaFunctionArn,
[property: CliOption("--account-default-for-operations")] string[] AccountDefaultForOperations
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}