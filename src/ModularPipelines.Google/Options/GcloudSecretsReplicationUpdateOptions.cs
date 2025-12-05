using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "replication", "update")]
public record GcloudSecretsReplicationUpdateOptions(
[property: CliArgument] string Secret
) : GcloudOptions
{
    [CliFlag("--remove-cmek")]
    public bool? RemoveCmek { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--set-kms-key")]
    public string? SetKmsKey { get; set; }
}