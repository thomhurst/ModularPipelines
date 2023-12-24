using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "terminate-clusters")]
public record AwsEmrTerminateClustersOptions(
[property: CommandSwitch("--cluster-ids")] string[] ClusterIds
) : AwsOptions;