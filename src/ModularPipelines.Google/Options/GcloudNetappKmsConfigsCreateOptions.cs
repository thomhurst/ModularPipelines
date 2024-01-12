using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "kms-configs", "create")]
public record GcloudNetappKmsConfigsCreateOptions(
[property: PositionalArgument] string KmsConfig,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--kms-key")] string KmsKey,
[property: CommandSwitch("--kms-keyring")] string KmsKeyring,
[property: CommandSwitch("--kms-location")] string KmsLocation,
[property: CommandSwitch("--kms-project")] string KmsProject
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}