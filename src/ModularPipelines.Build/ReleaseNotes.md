*   Breaking: `ConfigurePipelinePlugins` method has been removed and the methods made available directly on the PipelineHostBuilder
*   Breaking: `IPipelineModuleHooks.OnBeforeModuleEndAsync` has been renamed to `OnAfterModuleEndAsync`
*   Trying to retrieve a Module from within a RunConditionAttribute or a Pipeline Hook will no longer hang and will throw an exception
