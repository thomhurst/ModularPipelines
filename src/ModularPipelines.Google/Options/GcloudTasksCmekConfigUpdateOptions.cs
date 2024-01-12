using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "cmek-config", "update")]
public record GcloudTasksCmekConfigUpdateOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--clear-kms-key")]
    public bool? ClearKmsKey { get; set; }

    [CommandSwitch("--kms-key-name")]
    public string? KmsKeyName { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}