using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-dynamic-thing-group")]
public record AwsIotUpdateDynamicThingGroupOptions(
[property: CliOption("--thing-group-name")] string ThingGroupName,
[property: CliOption("--thing-group-properties")] string ThingGroupProperties
) : AwsOptions
{
    [CliOption("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CliOption("--index-name")]
    public string? IndexName { get; set; }

    [CliOption("--query-string")]
    public string? QueryString { get; set; }

    [CliOption("--query-version")]
    public string? QueryVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}