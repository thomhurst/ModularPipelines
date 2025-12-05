using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "upgrade-published-schema")]
public record AwsClouddirectoryUpgradePublishedSchemaOptions(
[property: CliOption("--development-schema-arn")] string DevelopmentSchemaArn,
[property: CliOption("--published-schema-arn")] string PublishedSchemaArn,
[property: CliOption("--minor-version")] string MinorVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}