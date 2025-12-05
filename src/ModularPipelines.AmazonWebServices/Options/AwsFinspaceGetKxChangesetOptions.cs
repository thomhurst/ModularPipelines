using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "get-kx-changeset")]
public record AwsFinspaceGetKxChangesetOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--changeset-id")] string ChangesetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}