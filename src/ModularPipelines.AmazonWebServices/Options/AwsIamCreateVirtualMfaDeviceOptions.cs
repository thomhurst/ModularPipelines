using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "create-virtual-mfa-device")]
public record AwsIamCreateVirtualMfaDeviceOptions(
[property: CliOption("--virtual-mfa-device-name")] string VirtualMfaDeviceName,
[property: CliOption("--outfile")] string Outfile,
[property: CliOption("--bootstrap-method")] string BootstrapMethod
) : AwsOptions
{
    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }
}