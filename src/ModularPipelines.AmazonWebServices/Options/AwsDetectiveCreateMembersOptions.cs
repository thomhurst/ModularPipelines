using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("detective", "create-members")]
public record AwsDetectiveCreateMembersOptions(
[property: CommandSwitch("--graph-arn")] string GraphArn,
[property: CommandSwitch("--accounts")] string[] Accounts
) : AwsOptions
{
    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}