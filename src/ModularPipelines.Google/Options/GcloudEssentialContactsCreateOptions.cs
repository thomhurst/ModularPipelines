using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("essential-contacts", "create")]
public record GcloudEssentialContactsCreateOptions(
[property: CliOption("--email")] string Email,
[property: CliOption("--language")] string Language,
[property: CliOption("--notification-categories")] string[] NotificationCategories
) : GcloudOptions
{
    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}