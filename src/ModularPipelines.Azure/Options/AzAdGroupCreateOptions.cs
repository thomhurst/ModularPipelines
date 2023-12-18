using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "create")]
public record AzAdGroupCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--mail-nickname")] string MailNickname
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}