using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "machine-images", "create")]
public record GcloudComputeMachineImagesCreateOptions(
[property: CliArgument] string Image,
[property: CliOption("--source-instance")] string SourceInstance
) : GcloudOptions
{
    [CliOption("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--guest-flush")]
    public bool? GuestFlush { get; set; }

    [CliFlag("--require-csek-key-create")]
    public bool? RequireCsekKeyCreate { get; set; }

    [CliOption("--source-disk-csek-key")]
    public string[]? SourceDiskCsekKey { get; set; }

    [CliOption("--source-instance-zone")]
    public string? SourceInstanceZone { get; set; }

    [CliOption("--storage-location")]
    public string? StorageLocation { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}