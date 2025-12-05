using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-template")]
public record AwsQuicksightUpdateTemplateOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--template-id")] string TemplateId
) : AwsOptions
{
    [CliOption("--source-entity")]
    public string? SourceEntity { get; set; }

    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--validation-strategy")]
    public string? ValidationStrategy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}