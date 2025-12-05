using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-crawler")]
public record AwsGlueUpdateCrawlerOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--targets")]
    public string? Targets { get; set; }

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

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}