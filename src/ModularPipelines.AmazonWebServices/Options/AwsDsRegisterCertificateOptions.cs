using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "register-certificate")]
public record AwsDsRegisterCertificateOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--certificate-data")] string CertificateData
) : AwsOptions
{
    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--client-cert-auth-settings")]
    public string? ClientCertAuthSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}