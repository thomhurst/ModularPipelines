using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "inventory", "list-keys")]
public record GcloudKmsInventoryListKeysOptions : GcloudOptions;