using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "upgrade-applied-schema")]
public record AwsClouddirectoryUpgradeAppliedSchemaOptions(
[property: CliOption("--published-schema-arn")] string PublishedSchemaArn,
[property: CliOption("--directory-arn")] string DirectoryArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}