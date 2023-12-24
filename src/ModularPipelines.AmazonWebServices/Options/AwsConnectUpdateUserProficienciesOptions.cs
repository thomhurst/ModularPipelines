using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-user-proficiencies")]
public record AwsConnectUpdateUserProficienciesOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--user-proficiencies")] string[] UserProficiencies
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}