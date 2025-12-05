using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "import-jobs", "describe")]
public record GcloudKmsImportJobsDescribeOptions(
[property: CliArgument] string ImportJob
) : GcloudOptions
{
    [CliOption("--attestation-file")]
    public string? AttestationFile { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}