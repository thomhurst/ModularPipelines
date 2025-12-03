using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "authorize")]
public record GcloudTransferAuthorizeOptions : GcloudOptions
{
    [CliFlag("--add-missing")]
    public bool? AddMissing { get; set; }

    [CliOption("--creds-file")]
    public string? CredsFile { get; set; }
}