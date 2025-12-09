using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fms", "put-apps-list")]
public record AwsFmsPutAppsListOptions(
[property: CliOption("--apps-list")] string AppsList
) : AwsOptions
{
    [CliOption("--tag-list")]
    public string[]? TagList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}