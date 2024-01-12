using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "replication", "update")]
public record GcloudSecretsReplicationUpdateOptions(
[property: PositionalArgument] string Secret
) : GcloudOptions
{
    [BooleanCommandSwitch("--remove-cmek")]
    public bool? RemoveCmek { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--set-kms-key")]
    public string? SetKmsKey { get; set; }
}