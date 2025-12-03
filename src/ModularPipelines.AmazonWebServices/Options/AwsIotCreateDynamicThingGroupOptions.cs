using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-dynamic-thing-group")]
public record AwsIotCreateDynamicThingGroupOptions(
[property: CliOption("--thing-group-name")] string ThingGroupName,
[property: CliOption("--query-string")] string QueryString
) : AwsOptions
{
    [CliOption("--thing-group-properties")]
    public string? ThingGroupProperties { get; set; }

    [CliOption("--index-name")]
    public string? IndexName { get; set; }

    [CliOption("--query-version")]
    public string? QueryVersion { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}