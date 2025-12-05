using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "storage-pools", "describe")]
public record GcloudNetappStoragePoolsDescribeOptions(
[property: CliArgument] string StoragePool,
[property: CliArgument] string Location
) : GcloudOptions;