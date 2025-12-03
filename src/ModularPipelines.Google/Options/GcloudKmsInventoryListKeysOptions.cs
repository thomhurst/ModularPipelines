using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "inventory", "list-keys")]
public record GcloudKmsInventoryListKeysOptions : GcloudOptions;