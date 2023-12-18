using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "activity-log", "list-categories")]
public record AzMonitorActivityLogListCategoriesOptions : AzOptions;