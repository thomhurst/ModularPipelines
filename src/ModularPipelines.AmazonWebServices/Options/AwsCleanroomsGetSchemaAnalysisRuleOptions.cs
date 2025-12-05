using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "get-schema-analysis-rule")]
public record AwsCleanroomsGetSchemaAnalysisRuleOptions(
[property: CliOption("--collaboration-identifier")] string CollaborationIdentifier,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}