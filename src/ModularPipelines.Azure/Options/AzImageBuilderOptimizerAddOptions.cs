using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("image", "builder", "optimizer", "add")]
public record AzImageBuilderOptimizerAddOptions : AzOptions
{
    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliFlag("--enable-vm-boot")]
    public bool? EnableVmBoot { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}