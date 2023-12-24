using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "import-client-branding")]
public record AwsWorkspacesImportClientBrandingOptions(
[property: CommandSwitch("--resource-id")] string ResourceId
) : AwsOptions
{
    [CommandSwitch("--device-type-windows")]
    public string? DeviceTypeWindows { get; set; }

    [CommandSwitch("--device-type-osx")]
    public string? DeviceTypeOsx { get; set; }

    [CommandSwitch("--device-type-android")]
    public string? DeviceTypeAndroid { get; set; }

    [CommandSwitch("--device-type-ios")]
    public string? DeviceTypeIos { get; set; }

    [CommandSwitch("--device-type-linux")]
    public string? DeviceTypeLinux { get; set; }

    [CommandSwitch("--device-type-web")]
    public string? DeviceTypeWeb { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}