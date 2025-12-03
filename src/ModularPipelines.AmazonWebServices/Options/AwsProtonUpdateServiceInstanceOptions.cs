using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "update-service-instance")]
public record AwsProtonUpdateServiceInstanceOptions(
[property: CliOption("--deployment-type")] string DeploymentType,
[property: CliOption("--name")] string Name,
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--spec")]
    public string? Spec { get; set; }

    [CliOption("--template-major-version")]
    public string? TemplateMajorVersion { get; set; }

    [CliOption("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}