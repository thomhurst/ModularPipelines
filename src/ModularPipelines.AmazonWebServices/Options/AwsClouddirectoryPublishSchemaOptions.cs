using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "publish-schema")]
public record AwsClouddirectoryPublishSchemaOptions(
[property: CliOption("--development-schema-arn")] string DevelopmentSchemaArn,
[property: CliOption("--schema-version")] string SchemaVersion
) : AwsOptions
{
    [CliOption("--minor-version")]
    public string? MinorVersion { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}