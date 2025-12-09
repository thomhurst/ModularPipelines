using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "create-thesaurus")]
public record AwsKendraCreateThesaurusOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--name")] string Name,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--source-s3-path")] string SourceS3Path
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}