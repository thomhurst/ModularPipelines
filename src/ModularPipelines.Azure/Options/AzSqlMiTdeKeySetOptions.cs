using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi", "tde-key", "set")]
public record AzSqlMiTdeKeySetOptions(
[property: CliOption("--server-key-type")] string ServerKeyType
) : AzOptions
{
    [CliFlag("--auto-rotation-enabled")]
    public bool? AutoRotationEnabled { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kid")]
    public string? Kid { get; set; }

    [CliOption("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}