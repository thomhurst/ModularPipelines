using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhubstrategy", "get-application-component-details")]
public record AwsMigrationhubstrategyGetApplicationComponentDetailsOptions(
[property: CliOption("--application-component-id")] string ApplicationComponentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}