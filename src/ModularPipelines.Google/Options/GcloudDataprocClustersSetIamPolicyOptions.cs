using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "clusters", "set-iam-policy")]
public record GcloudDataprocClustersSetIamPolicyOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;