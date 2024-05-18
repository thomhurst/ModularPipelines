*   Simplified the execution and storing of module results by utilising lazy execution

# Breaking

*   The `OnAfterExecute` Module hooks now accept a `ModuleResult<T>` parameter so you can see what the module returned
