using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "create-virtual-mfa-device")]
public record AwsIamCreateVirtualMfaDeviceOptions(
[property: CommandSwitch("--virtual-mfa-device-name")] string VirtualMfaDeviceName,
[property: CommandSwitch("--outfile")] string Outfile,
[property: CommandSwitch("--bootstrap-method")] string BootstrapMethod
) : AwsOptions
{
    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }
}