using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("essential-contacts", "update")]
public record GcloudEssentialContactsUpdateOptions(
[property: CliArgument] string ContactId
) : GcloudOptions
{
    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--notification-categories")]
    public string[]? NotificationCategories { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}