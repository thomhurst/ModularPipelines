using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "modify-cluster-attributes")]
public record AwsEmrModifyClusterAttributesOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId
) : AwsOptions;