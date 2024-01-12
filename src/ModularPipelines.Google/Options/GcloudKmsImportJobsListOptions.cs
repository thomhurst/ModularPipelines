using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "import-jobs", "list")]
public record GcloudKmsImportJobsListOptions : GcloudOptions
{
    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}