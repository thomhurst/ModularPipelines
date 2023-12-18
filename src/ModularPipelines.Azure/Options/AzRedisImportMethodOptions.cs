using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "import-method")]
public record AzRedisImportMethodOptions(
[property: CommandSwitch("--files")] string Files
) : AzOptions
{
    [CommandSwitch("--auth-method")]
    public string? AuthMethod { get; set; }

    [CommandSwitch("--file-format")]
    public string? FileFormat { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}