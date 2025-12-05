using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "auth", "update")]
public record AzContainerappAuthUpdateOptions : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CliOption("--custom-host-header")]
    public string? CustomHostHeader { get; set; }

    [CliOption("--custom-proto-header")]
    public string? CustomProtoHeader { get; set; }

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

    [CliOption("--sas-url-secret")]
    public string? SasUrlSecret { get; set; }

    [CliOption("--sas-url-secret-name")]
    public string? SasUrlSecretName { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--token-store")]
    public bool? TokenStore { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}