using ModularPipelines.Context;

namespace ModularPipelines.Helpers;

internal interface IModuleContextCreator
{
    IModuleContext CreateContext(Type moduleType);
}