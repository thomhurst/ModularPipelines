using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "import-client-branding")]
public record AwsWorkspacesImportClientBrandingOptions(
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--device-type-windows")]
    public string? DeviceTypeWindows { get; set; }

    [CliOption("--device-type-osx")]
    public string? DeviceTypeOsx { get; set; }

    [CliOption("--device-type-android")]
    public string? DeviceTypeAndroid { get; set; }

    [CliOption("--device-type-ios")]
    public string? DeviceTypeIos { get; set; }

    [CliOption("--device-type-linux")]
    public string? DeviceTypeLinux { get; set; }

    [CliOption("--device-type-web")]
    public string? DeviceTypeWeb { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}