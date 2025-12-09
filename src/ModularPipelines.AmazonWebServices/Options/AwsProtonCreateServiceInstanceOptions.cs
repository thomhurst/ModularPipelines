using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "create-service-instance")]
public record AwsProtonCreateServiceInstanceOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--spec")] string Spec
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--template-major-version")]
    public string? TemplateMajorVersion { get; set; }

    [CliOption("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}