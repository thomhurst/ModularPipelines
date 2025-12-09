using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-template")]
public record AwsQuicksightCreateTemplateOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--template-id")] string TemplateId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--permissions")]
    public string[]? Permissions { get; set; }

    [CliOption("--source-entity")]
    public string? SourceEntity { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--validation-strategy")]
    public string? ValidationStrategy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}