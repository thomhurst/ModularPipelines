using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-contacts", "accept-page")]
public record AwsSsmContactsAcceptPageOptions(
[property: CommandSwitch("--page-id")] string PageId,
[property: CommandSwitch("--accept-type")] string AcceptType,
[property: CommandSwitch("--accept-code")] string AcceptCode
) : AwsOptions
{
    [CommandSwitch("--contact-channel-id")]
    public string? ContactChannelId { get; set; }

    [CommandSwitch("--note")]
    public string? Note { get; set; }

    [CommandSwitch("--accept-code-validation")]
    public string? AcceptCodeValidation { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}