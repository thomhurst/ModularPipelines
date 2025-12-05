using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "application-default", "revoke")]
public record GcloudAuthApplicationDefaultRevokeOptions : GcloudOptions;