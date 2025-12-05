using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "enable-mfa-device")]
public record AwsIamEnableMfaDeviceOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--serial-number")] string SerialNumber,
[property: CliOption("--authentication-code1")] string AuthenticationCode1,
[property: CliOption("--authentication-code2")] string AuthenticationCode2
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}