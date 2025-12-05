using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "terminate-clusters")]
public record AwsEmrTerminateClustersOptions(
[property: CliOption("--cluster-ids")] string[] ClusterIds
) : AwsOptions;