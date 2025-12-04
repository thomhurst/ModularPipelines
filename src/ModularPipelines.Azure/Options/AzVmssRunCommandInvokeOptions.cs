using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "run-command", "invoke")]
public record AzVmssRunCommandInvokeOptions(
[property: CliOption("--command-id")] string CommandId
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scripts")]
    public string? Scripts { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}