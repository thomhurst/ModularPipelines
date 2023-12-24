using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "update-user-profile")]
public record AwsDatazoneUpdateUserProfileOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--status")] string Status,
[property: CommandSwitch("--user-identifier")] string UserIdentifier
) : AwsOptions
{
    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}