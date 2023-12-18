using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "policy-setting", "update")]
public record AzNetworkApplicationGatewayWafPolicyPolicySettingUpdateOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--custom-body")]
    public string? CustomBody { get; set; }

    [CommandSwitch("--custom-status-code")]
    public string? CustomStatusCode { get; set; }

    [BooleanCommandSwitch("--file-upload-enforce")]
    public bool? FileUploadEnforce { get; set; }

    [CommandSwitch("--file-upload-limit-in-mb")]
    public string? FileUploadLimitInMb { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--log-scrubbing-state")]
    public string? LogScrubbingState { get; set; }

    [CommandSwitch("--max-request-body-size-in-kb")]
    public string? MaxRequestBodySizeInKb { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [BooleanCommandSwitch("--request-body-check")]
    public bool? RequestBodyCheck { get; set; }

    [BooleanCommandSwitch("--request-body-enforce")]
    public bool? RequestBodyEnforce { get; set; }

    [CommandSwitch("--request-body-inspect-limit-in-kb")]
    public string? RequestBodyInspectLimitInKb { get; set; }

    [CommandSwitch("--scrubbing-rule")]
    public string? ScrubbingRule { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }
}

