using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-thing-group")]
public record AwsIotCreateThingGroupOptions(
[property: CliOption("--thing-group-name")] string ThingGroupName
) : AwsOptions
{
    [CliOption("--parent-group-name")]
    public string? ParentGroupName { get; set; }

    [CliOption("--thing-group-properties")]
    public string? ThingGroupProperties { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}