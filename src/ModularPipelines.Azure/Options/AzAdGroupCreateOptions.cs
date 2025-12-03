using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "group", "create")]
public record AzAdGroupCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--mail-nickname")] string MailNickname
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}