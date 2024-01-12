using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "storage-pools", "describe")]
public record GcloudNetappStoragePoolsDescribeOptions(
[property: PositionalArgument] string StoragePool,
[property: PositionalArgument] string Location
) : GcloudOptions;