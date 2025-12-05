using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-template-alias")]
public record AwsQuicksightUpdateTemplateAliasOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--template-id")] string TemplateId,
[property: CliOption("--alias-name")] string AliasName,
[property: CliOption("--template-version-number")] long TemplateVersionNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}