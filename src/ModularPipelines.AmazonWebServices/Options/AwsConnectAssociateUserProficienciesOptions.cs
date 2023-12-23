using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "associate-user-proficiencies")]
public record AwsConnectAssociateUserProficienciesOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--user-proficiencies")] string[] UserProficiencies
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}