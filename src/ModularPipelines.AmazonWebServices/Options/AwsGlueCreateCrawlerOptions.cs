using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-crawler")]
public record AwsGlueCreateCrawlerOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--targets")] string Targets
) : AwsOptions
{
    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--classifiers")]
    public string[]? Classifiers { get; set; }

    [CommandSwitch("--table-prefix")]
    public string? TablePrefix { get; set; }

    [CommandSwitch("--schema-change-policy")]
    public string? SchemaChangePolicy { get; set; }

    [CommandSwitch("--recrawl-policy")]
    public string? RecrawlPolicy { get; set; }

    [CommandSwitch("--lineage-configuration")]
    public string? LineageConfiguration { get; set; }

    [CommandSwitch("--lake-formation-configuration")]
    public string? LakeFormationConfiguration { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--crawler-security-configuration")]
    public string? CrawlerSecurityConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}