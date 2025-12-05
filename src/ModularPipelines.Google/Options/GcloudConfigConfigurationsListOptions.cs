using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "configurations", "list")]
public record GcloudConfigConfigurationsListOptions : GcloudOptions;