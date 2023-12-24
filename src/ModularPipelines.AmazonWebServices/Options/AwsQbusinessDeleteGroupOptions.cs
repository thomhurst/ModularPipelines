using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "delete-group")]
public record AwsQbusinessDeleteGroupOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--index-id")] string IndexId
) : AwsOptions
{
    [CommandSwitch("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}