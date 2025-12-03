using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "modify-cluster-attributes")]
public record AwsEmrModifyClusterAttributesOptions(
[property: CliOption("--cluster-id")] string ClusterId
) : AwsOptions;