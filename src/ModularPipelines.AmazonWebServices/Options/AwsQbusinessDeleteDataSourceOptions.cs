using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "delete-data-source")]
public record AwsQbusinessDeleteDataSourceOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--data-source-id")] string DataSourceId,
[property: CommandSwitch("--index-id")] string IndexId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}