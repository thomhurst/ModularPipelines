using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "kms-configs", "create")]
public record GcloudNetappKmsConfigsCreateOptions(
[property: CliArgument] string KmsConfig,
[property: CliArgument] string Location,
[property: CliOption("--kms-key")] string KmsKey,
[property: CliOption("--kms-keyring")] string KmsKeyring,
[property: CliOption("--kms-location")] string KmsLocation,
[property: CliOption("--kms-project")] string KmsProject
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}