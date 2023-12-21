using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "hostname", "bind")]
public record AzContainerappHostnameBindOptions(
[property: CommandSwitch("--hostname")] string Hostname
) : AzOptions
{
    [CommandSwitch("--certificate")]
    public string? Certificate { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--thumbprint")]
    public string? Thumbprint { get; set; }

    [CommandSwitch("--validation-method")]
    public string? ValidationMethod { get; set; }
}