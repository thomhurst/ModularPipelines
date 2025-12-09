using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "tde-key", "set")]
public record AzSqlServerTdeKeySetOptions(
[property: CliOption("--server-key-type")] string ServerKeyType
) : AzOptions
{
    [CliFlag("--auto-rotation-enabled")]
    public bool? AutoRotationEnabled { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kid")]
    public string? Kid { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}