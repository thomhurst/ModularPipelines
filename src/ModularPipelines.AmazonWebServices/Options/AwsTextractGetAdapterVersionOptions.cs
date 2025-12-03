using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("textract", "get-adapter-version")]
public record AwsTextractGetAdapterVersionOptions(
[property: CliOption("--adapter-id")] string AdapterId,
[property: CliOption("--adapter-version")] string AdapterVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}