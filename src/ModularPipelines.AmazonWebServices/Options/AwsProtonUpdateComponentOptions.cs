using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "update-component")]
public record AwsProtonUpdateComponentOptions(
[property: CliOption("--deployment-type")] string DeploymentType,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--service-instance-name")]
    public string? ServiceInstanceName { get; set; }

    [CliOption("--service-name")]
    public string? ServiceName { get; set; }

    [CliOption("--service-spec")]
    public string? ServiceSpec { get; set; }

    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}