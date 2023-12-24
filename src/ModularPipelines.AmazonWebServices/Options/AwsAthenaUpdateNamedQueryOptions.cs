using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "update-named-query")]
public record AwsAthenaUpdateNamedQueryOptions(
[property: CommandSwitch("--named-query-id")] string NamedQueryId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--query-string")] string QueryString
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}