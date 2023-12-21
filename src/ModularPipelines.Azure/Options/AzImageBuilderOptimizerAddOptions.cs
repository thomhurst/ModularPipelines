using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("image", "builder", "optimizer", "add")]
public record AzImageBuilderOptimizerAddOptions : AzOptions
{
    [CommandSwitch("--defer")]
    public string? Defer { get; set; }

    [BooleanCommandSwitch("--enable-vm-boot")]
    public bool? EnableVmBoot { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}