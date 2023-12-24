using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "put-retention-policy")]
public record AwsWorkmailPutRetentionPolicyOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--folder-configurations")] string[] FolderConfigurations
) : AwsOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}