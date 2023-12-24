using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "remove-user-from-group")]
public record AwsIamRemoveUserFromGroupOptions(
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--user-name")] string UserName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}