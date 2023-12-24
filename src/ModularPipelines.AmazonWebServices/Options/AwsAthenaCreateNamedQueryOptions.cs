using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "create-named-query")]
public record AwsAthenaCreateNamedQueryOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--query-string")] string QueryString
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--work-group")]
    public string? WorkGroup { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}