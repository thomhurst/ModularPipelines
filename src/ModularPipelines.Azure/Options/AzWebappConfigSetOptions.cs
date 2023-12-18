using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "set")]
public record AzWebappConfigSetOptions : AzOptions
{
    [BooleanCommandSwitch("--always-on")]
    public bool? AlwaysOn { get; set; }

    [BooleanCommandSwitch("--auto-heal-enabled")]
    public bool? AutoHealEnabled { get; set; }

    [CommandSwitch("--ftps-state")]
    public string? FtpsState { get; set; }

    [CommandSwitch("--generic-configurations")]
    public string? GenericConfigurations { get; set; }

    [BooleanCommandSwitch("--http20-enabled")]
    public bool? Http20Enabled { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--java-container")]
    public string? JavaContainer { get; set; }

    [CommandSwitch("--java-container-version")]
    public string? JavaContainerVersion { get; set; }

    [CommandSwitch("--java-version")]
    public string? JavaVersion { get; set; }

    [CommandSwitch("--linux-fx-version")]
    public string? LinuxFxVersion { get; set; }

    [CommandSwitch("--min-tls-version")]
    public string? MinTlsVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--net-framework-version")]
    public string? NetFrameworkVersion { get; set; }

    [CommandSwitch("--number-of-workers")]
    public string? NumberOfWorkers { get; set; }

    [CommandSwitch("--php-version")]
    public string? PhpVersion { get; set; }

    [CommandSwitch("--powershell-version")]
    public string? PowershellVersion { get; set; }

    [CommandSwitch("--prewarmed-instance-count")]
    public int? PrewarmedInstanceCount { get; set; }

    [CommandSwitch("--python-version")]
    public string? PythonVersion { get; set; }

    [BooleanCommandSwitch("--remote-debugging-enabled")]
    public bool? RemoteDebuggingEnabled { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--startup-file")]
    public string? StartupFile { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--use-32bit-worker-process")]
    public bool? Use32bitWorkerProcess { get; set; }

    [BooleanCommandSwitch("--vnet-route-all-enabled")]
    public bool? VnetRouteAllEnabled { get; set; }

    [BooleanCommandSwitch("--web-sockets-enabled")]
    public bool? WebSocketsEnabled { get; set; }

    [CommandSwitch("--windows-fx-version")]
    public string? WindowsFxVersion { get; set; }
}