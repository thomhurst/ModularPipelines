using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dls", "fs", "move")]
public record AzDlsFsMoveOptions(
[property: CliOption("--destination-path")] string DestinationPath,
[property: CliOption("--source-path")] string SourcePath
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}