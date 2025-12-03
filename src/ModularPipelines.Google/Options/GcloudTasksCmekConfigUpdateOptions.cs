using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "cmek-config", "update")]
public record GcloudTasksCmekConfigUpdateOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--clear-kms-key")]
    public bool? ClearKmsKey { get; set; }

    [CliOption("--kms-key-name")]
    public string? KmsKeyName { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}