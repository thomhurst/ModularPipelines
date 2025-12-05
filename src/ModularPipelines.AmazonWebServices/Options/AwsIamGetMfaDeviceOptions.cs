using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "get-mfa-device")]
public record AwsIamGetMfaDeviceOptions(
[property: CliOption("--serial-number")] string SerialNumber
) : AwsOptions
{
    [CliOption("--user-name")]
    public string? UserName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}