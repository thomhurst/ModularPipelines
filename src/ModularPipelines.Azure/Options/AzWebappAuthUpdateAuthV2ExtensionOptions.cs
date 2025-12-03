using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "auth", "update", "(authV2", "extension)")]
public record AzWebappAuthUpdateAuthV2ExtensionOptions : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CliOption("--custom-host-header")]
    public string? CustomHostHeader { get; set; }

    [CliOption("--custom-proto-header")]
    public string? CustomProtoHeader { get; set; }

    [CliFlag("--enable-token-store")]
    public bool? EnableTokenStore { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--excluded-paths")]
    public string? ExcludedPaths { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--proxy-convention")]
    public string? ProxyConvention { get; set; }

    [CliOption("--redirect-provider")]
    public string? RedirectProvider { get; set; }

    [CliFlag("--require-https")]
    public bool? RequireHttps { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}