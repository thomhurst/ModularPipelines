using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "create-alias")]
public record AwsWorkmailCreateAliasOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--entity-id")] string EntityId,
[property: CommandSwitch("--alias")] string Alias
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}