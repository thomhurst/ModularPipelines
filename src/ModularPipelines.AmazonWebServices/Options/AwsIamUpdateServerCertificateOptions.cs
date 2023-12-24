using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "update-server-certificate")]
public record AwsIamUpdateServerCertificateOptions(
[property: CommandSwitch("--server-certificate-name")] string ServerCertificateName
) : AwsOptions
{
    [CommandSwitch("--new-path")]
    public string? NewPath { get; set; }

    [CommandSwitch("--new-server-certificate-name")]
    public string? NewServerCertificateName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}