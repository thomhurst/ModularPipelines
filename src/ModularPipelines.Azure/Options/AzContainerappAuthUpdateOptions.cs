using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "auth", "update")]
public record AzContainerappAuthUpdateOptions : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CommandSwitch("--custom-host-header")]
    public string? CustomHostHeader { get; set; }

    [CommandSwitch("--custom-proto-header")]
    public string? CustomProtoHeader { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--excluded-paths")]
    public string? ExcludedPaths { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--proxy-convention")]
    public string? ProxyConvention { get; set; }

    [CommandSwitch("--redirect-provider")]
    public string? RedirectProvider { get; set; }

    [BooleanCommandSwitch("--require-https")]
    public bool? RequireHttps { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--sas-url-secret")]
    public string? SasUrlSecret { get; set; }

    [CommandSwitch("--sas-url-secret-name")]
    public string? SasUrlSecretName { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--token-store")]
    public bool? TokenStore { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}