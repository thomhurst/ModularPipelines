using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dls", "fs", "access", "remove-entry")]
public record AzDlsFsAccessRemoveEntryOptions(
[property: CliOption("--acl-spec")] string AclSpec,
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}