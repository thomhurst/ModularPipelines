using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "create-kx-changeset")]
public record AwsFinspaceCreateKxChangesetOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--change-requests")] string[] ChangeRequests
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}