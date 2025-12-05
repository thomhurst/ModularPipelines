using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyspaces", "delete-keyspace")]
public record AwsKeyspacesDeleteKeyspaceOptions(
[property: CliOption("--keyspace-name")] string KeyspaceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}