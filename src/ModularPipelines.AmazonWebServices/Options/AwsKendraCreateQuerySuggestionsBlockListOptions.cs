using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "create-query-suggestions-block-list")]
public record AwsKendraCreateQuerySuggestionsBlockListOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--name")] string Name,
[property: CliOption("--source-s3-path")] string SourceS3Path,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}