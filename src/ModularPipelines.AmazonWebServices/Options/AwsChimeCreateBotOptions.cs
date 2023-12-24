using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "create-bot")]
public record AwsChimeCreateBotOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--display-name")] string DisplayName
) : AwsOptions
{
    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}