using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("textract", "update-adapter")]
public record AwsTextractUpdateAdapterOptions(
[property: CliOption("--adapter-id")] string AdapterId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--adapter-name")]
    public string? AdapterName { get; set; }

    [CliOption("--auto-update")]
    public string? AutoUpdate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}