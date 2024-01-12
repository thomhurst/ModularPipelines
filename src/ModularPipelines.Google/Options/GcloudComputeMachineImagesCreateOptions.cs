using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "machine-images", "create")]
public record GcloudComputeMachineImagesCreateOptions(
[property: PositionalArgument] string Image,
[property: CommandSwitch("--source-instance")] string SourceInstance
) : GcloudOptions
{
    [CommandSwitch("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--guest-flush")]
    public bool? GuestFlush { get; set; }

    [BooleanCommandSwitch("--require-csek-key-create")]
    public bool? RequireCsekKeyCreate { get; set; }

    [CommandSwitch("--source-disk-csek-key")]
    public string[]? SourceDiskCsekKey { get; set; }

    [CommandSwitch("--source-instance-zone")]
    public string? SourceInstanceZone { get; set; }

    [CommandSwitch("--storage-location")]
    public string? StorageLocation { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}