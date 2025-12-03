using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "ssh-keys", "list")]
public record GcloudBmsSshKeysListOptions : GcloudOptions;