using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "services", "update")]
public record GcloudAppServicesUpdateOptions(
[property: CliArgument] string Services,
[property: CliOption("--ingress")] string Ingress
) : GcloudOptions;