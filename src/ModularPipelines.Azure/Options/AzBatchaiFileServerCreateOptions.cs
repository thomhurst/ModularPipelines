using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai", "file-server", "create")]
public record AzBatchaiFileServerCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace")] string Workspace
) : AzOptions
{
    [CommandSwitch("--caching-type")]
    public string? CachingType { get; set; }

    [CommandSwitch("--config-file")]
    public string? ConfigFile { get; set; }

    [CommandSwitch("--disk-count")]
    public int? DiskCount { get; set; }

    [CommandSwitch("--disk-size")]
    public string? DiskSize { get; set; }

    [CommandSwitch("--generate-ssh-keys")]
    public string? GenerateSshKeys { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--ssh-key")]
    public string? SshKey { get; set; }

    [CommandSwitch("--storage-sku")]
    public string? StorageSku { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }
}

