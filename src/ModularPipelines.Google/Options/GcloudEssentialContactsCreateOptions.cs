using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("essential-contacts", "create")]
public record GcloudEssentialContactsCreateOptions(
[property: CommandSwitch("--email")] string Email,
[property: CommandSwitch("--language")] string Language,
[property: CommandSwitch("--notification-categories")] string[] NotificationCategories
) : GcloudOptions
{
    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}