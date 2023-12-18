using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "nv", "create")]
public record AzApimNvCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--named-value-id")] string NamedValueId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--secret")]
    public bool? Secret { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }
}