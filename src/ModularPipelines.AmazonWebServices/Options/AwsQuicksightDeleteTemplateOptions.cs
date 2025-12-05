using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-template")]
public record AwsQuicksightDeleteTemplateOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--template-id")] string TemplateId
) : AwsOptions
{
    [CliOption("--version-number")]
    public long? VersionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}