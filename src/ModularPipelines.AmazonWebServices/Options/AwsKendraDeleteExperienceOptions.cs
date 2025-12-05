using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "delete-experience")]
public record AwsKendraDeleteExperienceOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}