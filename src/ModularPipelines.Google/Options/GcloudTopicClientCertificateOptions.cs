using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("topic", "client-certificate")]
public record GcloudTopicClientCertificateOptions : GcloudOptions;