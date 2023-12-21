using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "tde-key", "set")]
public record AzSqlServerTdeKeySetOptions(
[property: CommandSwitch("--server-key-type")] string ServerKeyType
) : AzOptions
{
    [BooleanCommandSwitch("--auto-rotation-enabled")]
    public bool? AutoRotationEnabled { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kid")]
    public string? Kid { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}