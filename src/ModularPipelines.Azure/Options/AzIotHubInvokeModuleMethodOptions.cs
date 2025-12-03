using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "invoke-module-method")]
public record AzIotHubInvokeModuleMethodOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--method-name")] string MethodName,
[property: CliOption("--module-id")] string ModuleId
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--method-payload")]
    public string? MethodPayload { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}