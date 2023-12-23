using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "put-query-definition")]
public record AwsLogsPutQueryDefinitionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--query-string")] string QueryString
) : AwsOptions
{
    [CommandSwitch("--query-definition-id")]
    public string? QueryDefinitionId { get; set; }

    [CommandSwitch("--log-group-names")]
    public string[]? LogGroupNames { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}