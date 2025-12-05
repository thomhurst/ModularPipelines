using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyspaces", "get-keyspace")]
public record AwsKeyspacesGetKeyspaceOptions(
[property: CliOption("--keyspace-name")] string KeyspaceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}