using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-certificate-provider")]
public record AwsIotCreateCertificateProviderOptions(
[property: CommandSwitch("--certificate-provider-name")] string CertificateProviderName,
[property: CommandSwitch("--lambda-function-arn")] string LambdaFunctionArn,
[property: CommandSwitch("--account-default-for-operations")] string[] AccountDefaultForOperations
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}