using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "create-named-query")]
public record AwsAthenaCreateNamedQueryOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--database")] string Database,
[property: CliOption("--query-string")] string QueryString
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--work-group")]
    public string? WorkGroup { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}