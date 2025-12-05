using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "get-kx-database")]
public record AwsFinspaceGetKxDatabaseOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--database-name")] string DatabaseName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}