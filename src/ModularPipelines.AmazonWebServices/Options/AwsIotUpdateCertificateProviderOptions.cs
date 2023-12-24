using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-certificate-provider")]
public record AwsIotUpdateCertificateProviderOptions(
[property: CommandSwitch("--certificate-provider-name")] string CertificateProviderName
) : AwsOptions
{
    [CommandSwitch("--lambda-function-arn")]
    public string? LambdaFunctionArn { get; set; }

    [CommandSwitch("--account-default-for-operations")]
    public string[]? AccountDefaultForOperations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}