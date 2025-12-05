using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "clusters", "get-cluster-certificate-authority")]
public record GcloudRedisClustersGetClusterCertificateAuthorityOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions;