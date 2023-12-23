using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("b2bi", "create-profile")]
public record AwsB2biCreateProfileOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--phone")] string Phone,
[property: CommandSwitch("--business-name")] string BusinessName,
[property: CommandSwitch("--logging")] string Logging
) : AwsOptions
{
    [CommandSwitch("--email")]
    public string? Email { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}