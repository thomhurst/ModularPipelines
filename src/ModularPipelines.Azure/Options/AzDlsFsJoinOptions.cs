using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dls", "fs", "join")]
public record AzDlsFsJoinOptions(
[property: CommandSwitch("--destination-path")] string DestinationPath,
[property: CommandSwitch("--source-paths")] string SourcePaths
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

