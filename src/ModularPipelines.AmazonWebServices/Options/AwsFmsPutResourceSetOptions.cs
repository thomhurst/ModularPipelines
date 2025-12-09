using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fms", "put-resource-set")]
public record AwsFmsPutResourceSetOptions(
[property: CliOption("--resource-set")] string ResourceSet
) : AwsOptions
{
    [CliOption("--tag-list")]
    public string[]? TagList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}