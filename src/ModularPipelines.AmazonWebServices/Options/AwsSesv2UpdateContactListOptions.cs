using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "update-contact-list")]
public record AwsSesv2UpdateContactListOptions(
[property: CommandSwitch("--contact-list-name")] string ContactListName
) : AwsOptions
{
    [CommandSwitch("--topics")]
    public string[]? Topics { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}