using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "update-crawler")]
public record AwsGlueUpdateCrawlerOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--targets")]
    public string? Targets { get; set; }

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

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}