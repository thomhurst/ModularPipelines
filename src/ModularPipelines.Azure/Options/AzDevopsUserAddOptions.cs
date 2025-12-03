using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops", "user", "add")]
public record AzDevopsUserAddOptions(
[property: CliOption("--email-id")] string EmailId,
[property: CliOption("--license-type")] string LicenseType
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliFlag("--send-email-invite")]
    public bool? SendEmailInvite { get; set; }
}