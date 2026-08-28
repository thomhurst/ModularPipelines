using ModularPipelines.Models;
using ModularPipelines.Reporting;

namespace ModularPipelines.Console;

internal interface IModuleOutputExcerptProvider
{
    ModuleOutputExcerpt? GetModuleOutputExcerpt(Type moduleType);
}
