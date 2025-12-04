using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dls", "fs", "create")]
public record AzDlsFsCreateOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--content")]
    public string? Content { get; set; }

    [CliFlag("--folder")]
    public bool? Folder { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}