using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "modify-instance-groups")]
public record AwsEmrModifyInstanceGroupsOptions : AwsOptions
{
    [CliOption("--cluster-id")]
    public string? ClusterId { get; set; }

    [CliOption("--instance-groups")]
    public string[]? InstanceGroups { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}