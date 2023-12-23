using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "add-data-source")]
public record AwsOpensearchAddDataSourceOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--data-source-type")] string DataSourceType
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}