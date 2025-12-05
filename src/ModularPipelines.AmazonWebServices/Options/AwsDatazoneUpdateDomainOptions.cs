using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "update-domain")]
public record AwsDatazoneUpdateDomainOptions(
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--domain-execution-role")]
    public string? DomainExecutionRole { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--single-sign-on")]
    public string? SingleSignOn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}