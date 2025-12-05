using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "run-command", "show")]
public record AzVmRunCommandShowOptions : AzOptions
{
    [CliOption("--command-id")]
    public string? CommandId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--instance-view")]
    public bool? InstanceView { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vm-name")]
    public string? VmName { get; set; }
}