using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-template-alias")]
public record AwsQuicksightDescribeTemplateAliasOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--template-id")] string TemplateId,
[property: CliOption("--alias-name")] string AliasName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}