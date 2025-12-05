using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-crawler")]
public record AwsGlueCreateCrawlerOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role")] string Role,
[property: CliOption("--targets")] string Targets
) : AwsOptions
{
    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--classifiers")]
    public string[]? Classifiers { get; set; }

    [CliOption("--table-prefix")]
    public string? TablePrefix { get; set; }

    [CliOption("--schema-change-policy")]
    public string? SchemaChangePolicy { get; set; }

    [CliOption("--recrawl-policy")]
    public string? RecrawlPolicy { get; set; }

    [CliOption("--lineage-configuration")]
    public string? LineageConfiguration { get; set; }

    [CliOption("--lake-formation-configuration")]
    public string? LakeFormationConfiguration { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--crawler-security-configuration")]
    public string? CrawlerSecurityConfiguration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}