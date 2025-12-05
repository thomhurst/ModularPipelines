using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "assign-instance")]
public record AwsOpsworksAssignInstanceOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--layer-ids")] string[] LayerIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}