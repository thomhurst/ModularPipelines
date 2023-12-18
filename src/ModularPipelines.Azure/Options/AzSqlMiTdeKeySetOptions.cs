using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi", "tde-key", "set")]
public record AzSqlMiTdeKeySetOptions(
[property: CommandSwitch("--server-key-type")] string ServerKeyType
) : AzOptions
{
    [BooleanCommandSwitch("--auto-rotation-enabled")]
    public bool? AutoRotationEnabled { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kid")]
    public string? Kid { get; set; }

    [CommandSwitch("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}