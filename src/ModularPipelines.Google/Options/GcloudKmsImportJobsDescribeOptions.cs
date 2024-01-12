using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "import-jobs", "describe")]
public record GcloudKmsImportJobsDescribeOptions(
[property: PositionalArgument] string ImportJob
) : GcloudOptions
{
    [CommandSwitch("--attestation-file")]
    public string? AttestationFile { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}