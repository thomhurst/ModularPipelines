using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "user", "add")]
public record AzDevopsUserAddOptions(
[property: CommandSwitch("--email-id")] string EmailId,
[property: CommandSwitch("--license-type")] string LicenseType
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [BooleanCommandSwitch("--send-email-invite")]
    public bool? SendEmailInvite { get; set; }
}

