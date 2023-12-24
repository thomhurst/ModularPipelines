using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("b2bi", "create-partnership")]
public record AwsB2biCreatePartnershipOptions(
[property: CommandSwitch("--profile-id")] string ProfileId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--email")] string Email
) : AwsOptions
{
    [CommandSwitch("--phone")]
    public string? Phone { get; set; }

    [CommandSwitch("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}