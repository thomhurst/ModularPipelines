using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("textract", "delete-adapter-version")]
public record AwsTextractDeleteAdapterVersionOptions(
[property: CliOption("--adapter-id")] string AdapterId,
[property: CliOption("--adapter-version")] string AdapterVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}