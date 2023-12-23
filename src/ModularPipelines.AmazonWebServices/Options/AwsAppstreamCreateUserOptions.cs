using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-user")]
public record AwsAppstreamCreateUserOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--authentication-type")] string AuthenticationType
) : AwsOptions
{
    [CommandSwitch("--message-action")]
    public string? MessageAction { get; set; }

    [CommandSwitch("--first-name")]
    public string? FirstName { get; set; }

    [CommandSwitch("--last-name")]
    public string? LastName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}