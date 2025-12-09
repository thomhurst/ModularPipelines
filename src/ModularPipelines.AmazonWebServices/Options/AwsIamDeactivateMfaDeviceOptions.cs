using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "deactivate-mfa-device")]
public record AwsIamDeactivateMfaDeviceOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--serial-number")] string SerialNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}