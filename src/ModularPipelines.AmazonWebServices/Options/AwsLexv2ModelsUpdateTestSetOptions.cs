using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "update-test-set")]
public record AwsLexv2ModelsUpdateTestSetOptions(
[property: CliOption("--test-set-id")] string TestSetId,
[property: CliOption("--test-set-name")] string TestSetName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}