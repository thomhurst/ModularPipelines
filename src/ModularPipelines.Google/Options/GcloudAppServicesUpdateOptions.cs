using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "services", "update")]
public record GcloudAppServicesUpdateOptions(
[property: PositionalArgument] string Services,
[property: CommandSwitch("--ingress")] string Ingress
) : GcloudOptions;