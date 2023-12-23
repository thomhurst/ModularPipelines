using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-hsm-client-certificate")]
public record AwsRedshiftCreateHsmClientCertificateOptions(
[property: CommandSwitch("--hsm-client-certificate-identifier")] string HsmClientCertificateIdentifier
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}