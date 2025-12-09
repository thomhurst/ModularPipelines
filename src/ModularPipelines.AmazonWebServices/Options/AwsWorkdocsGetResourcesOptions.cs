using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "get-resources")]
public record AwsWorkdocsGetResourcesOptions : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }

    [CliOption("--collection-type")]
    public string? CollectionType { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}