using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "accept-predictions")]
public record AwsDatazoneAcceptPredictionsOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--identifier")] string Identifier
) : AwsOptions
{
    [CommandSwitch("--accept-choices")]
    public string[]? AcceptChoices { get; set; }

    [CommandSwitch("--accept-rule")]
    public string? AcceptRule { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}