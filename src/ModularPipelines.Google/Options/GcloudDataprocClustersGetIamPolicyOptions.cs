using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "clusters", "get-iam-policy")]
public record GcloudDataprocClustersGetIamPolicyOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions;