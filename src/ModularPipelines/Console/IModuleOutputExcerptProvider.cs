using ModularPipelines.Models;

namespace ModularPipelines.Console;

internal interface IModuleOutputExcerptProvider
{
    ModuleOutputExcerpt? GetModuleOutputExcerpt(Type moduleType);
}
