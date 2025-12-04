using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("urestackhci", "virtualmachine", "extension", "delete")]
public record AzAzurestackhciVirtualmachineExtensionDeleteOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--virtualmachine-name")]
    public string? VirtualmachineName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}