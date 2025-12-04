using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "waf-policy", "policy-setting", "update")]
public record AzNetworkApplicationGatewayWafPolicyPolicySettingUpdateOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--custom-body")]
    public string? CustomBody { get; set; }

    [CliOption("--custom-status-code")]
    public string? CustomStatusCode { get; set; }

    [CliFlag("--file-upload-enforce")]
    public bool? FileUploadEnforce { get; set; }

    [CliOption("--file-upload-limit-in-mb")]
    public string? FileUploadLimitInMb { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--log-scrubbing-state")]
    public string? LogScrubbingState { get; set; }

    [CliOption("--max-request-body-size-in-kb")]
    public string? MaxRequestBodySizeInKb { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliFlag("--request-body-check")]
    public bool? RequestBodyCheck { get; set; }

    [CliFlag("--request-body-enforce")]
    public bool? RequestBodyEnforce { get; set; }

    [CliOption("--request-body-inspect-limit-in-kb")]
    public string? RequestBodyInspectLimitInKb { get; set; }

    [CliOption("--scrubbing-rule")]
    public string? ScrubbingRule { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }
}