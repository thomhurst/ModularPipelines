using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "config", "set")]
public record AzFunctionappConfigSetOptions : AzOptions
{
    [CliFlag("--always-on")]
    public bool? AlwaysOn { get; set; }

    [CliFlag("--auto-heal-enabled")]
    public bool? AutoHealEnabled { get; set; }

    [CliOption("--ftps-state")]
    public string? FtpsState { get; set; }

    [CliOption("--generic-configurations")]
    public string? GenericConfigurations { get; set; }

    [CliFlag("--http20-enabled")]
    public bool? Http20Enabled { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--java-container")]
    public string? JavaContainer { get; set; }

    [CliOption("--java-container-version")]
    public string? JavaContainerVersion { get; set; }

    [CliOption("--java-version")]
    public string? JavaVersion { get; set; }

    [CliOption("--linux-fx-version")]
    public string? LinuxFxVersion { get; set; }

    [CliOption("--min-tls-version")]
    public string? MinTlsVersion { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--net-framework-version")]
    public string? NetFrameworkVersion { get; set; }

    [CliOption("--number-of-workers")]
    public string? NumberOfWorkers { get; set; }

    [CliOption("--php-version")]
    public string? PhpVersion { get; set; }

    [CliOption("--powershell-version")]
    public string? PowershellVersion { get; set; }

    [CliOption("--prewarmed-instance-count")]
    public int? PrewarmedInstanceCount { get; set; }

    [CliOption("--python-version")]
    public string? PythonVersion { get; set; }

    [CliFlag("--remote-debugging-enabled")]
    public bool? RemoteDebuggingEnabled { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--startup-file")]
    public string? StartupFile { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--use-32bit-worker-process")]
    public bool? Use32bitWorkerProcess { get; set; }

    [CliFlag("--vnet-route-all-enabled")]
    public bool? VnetRouteAllEnabled { get; set; }

    [CliFlag("--web-sockets-enabled")]
    public bool? WebSocketsEnabled { get; set; }
}