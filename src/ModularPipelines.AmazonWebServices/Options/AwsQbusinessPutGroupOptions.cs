using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "put-group")]
public record AwsQbusinessPutGroupOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--group-members")] string GroupMembers,
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}