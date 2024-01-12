using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "authorize")]
public record GcloudTransferAuthorizeOptions : GcloudOptions
{
    [BooleanCommandSwitch("--add-missing")]
    public bool? AddMissing { get; set; }

    [CommandSwitch("--creds-file")]
    public string? CredsFile { get; set; }
}