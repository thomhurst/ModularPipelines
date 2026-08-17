# Analyzer rules

| Rule                                                            | Category | Default severity | Availability                | Description                                                                     |
| --------------------------------------------------------------- | -------- | ---------------- | --------------------------- | ------------------------------------------------------------------------------- |
| [`MP0001`](/ModularPipelines/docs/next/analyzers/MP0001.md)     | Usage    | Error            | Public analyzer package     | Accesses a module result without declaring the dependency.                      |
| [`MP0002`](/ModularPipelines/docs/next/analyzers/MP0002.md)     | Usage    | Error            | Public analyzer package     | Uses IEnumerable as a module result instead of a concrete collection.           |
| [`MP0003`](/ModularPipelines/docs/next/analyzers/MP0003.md)     | Usage    | Error            | Public analyzer package     | Injects a logger into a module constructor instead of using the module context. |
| [`MP0004`](/ModularPipelines/docs/next/analyzers/MP0004.md)     | Usage    | Error            | Public analyzer package     | Writes directly to System.Console instead of using pipeline logging.            |
| [`MP0005`](/ModularPipelines/docs/next/analyzers/MP0005.md)     | Usage    | Error            | Public analyzer package     | Creates a circular dependency between modules.                                  |
| [`MP0006`](/ModularPipelines/docs/next/analyzers/MP0006.md)     | Usage    | Error            | Public analyzer package     | Implements ExecuteAsync without the async modifier.                             |
| [`MP0007`](/ModularPipelines/docs/next/analyzers/MP0007.md)     | Usage    | Error            | Public analyzer package     | Awaits the current module instance.                                             |
| [`MP0008`](/ModularPipelines/docs/next/analyzers/MP0008.md)     | Design   | Warning          | Public analyzer package     | Declares mutable instance state that can leak between module executions.        |
| [`MP0009`](/ModularPipelines/docs/next/analyzers/MP0009.md)     | Usage    | Error            | Public analyzer package     | References a dependency type that does not implement IModule.                   |
| [`MP0010`](/ModularPipelines/docs/next/analyzers/MP0010.md)     | Usage    | Error            | Public analyzer package     | Declares a module dependency on itself.                                         |
| [`MP0011`](/ModularPipelines/docs/next/analyzers/MP0011.md)     | Usage    | Warning          | Repository development only | Requires generated CLI option properties to be virtual.                         |
| [`MP0012`](/ModularPipelines/docs/next/analyzers/MP0012.md)     | Usage    | Warning          | Repository development only | Requires generated CLI command methods to be virtual.                           |
| [`MP0013`](/ModularPipelines/docs/next/analyzers/MP0013.md)     | Usage    | Warning          | Public analyzer package     | Module is not registered with the pipeline.                                     |
| [`MP0014`](/ModularPipelines/docs/next/analyzers/MP0014.md)     | Usage    | Warning          | Public analyzer package     | Async void method in a module.                                                  |
| [`MP0015`](/ModularPipelines/docs/next/analyzers/MP0015.md)     | Usage    | Warning          | Public analyzer package     | Blocking call in ExecuteAsync.                                                  |
| [`MP0016`](/ModularPipelines/docs/next/analyzers/MP0016.md)     | Usage    | Warning          | Public analyzer package     | ExecuteAsync cancellation token is not flowed.                                  |
| [`MP0017`](/ModularPipelines/docs/next/analyzers/MP0017.md)     | Usage    | Warning          | Public analyzer package     | Thread.Sleep in ExecuteAsync.                                                   |
| [`MP0018`](/ModularPipelines/docs/next/analyzers/MP0018.md)     | Usage    | Warning          | Public analyzer package     | Module class is not public.                                                     |
| [`MP0019`](/ModularPipelines/docs/next/analyzers/MP0019.md)     | Usage    | Warning          | Public analyzer package     | Duplicate DependsOn declaration.                                                |
| [`MPCLI001`](/ModularPipelines/docs/next/analyzers/MPCLI001.md) | Usage    | Error            | Public analyzer package     | CliFlag property must be bool? or int?                                          |
| [`MPCLI002`](/ModularPipelines/docs/next/analyzers/MPCLI002.md) | Usage    | Error            | Public analyzer package     | Value-less bool? CliOption should use CliFlag.                                  |
| [`MPCLI003`](/ModularPipelines/docs/next/analyzers/MPCLI003.md) | Usage    | Error            | Public analyzer package     | Multiple CLI attributes applied to one property.                                |
| [`MPCLI004`](/ModularPipelines/docs/next/analyzers/MPCLI004.md) | Usage    | Error            | Public analyzer package     | Duplicate CLI switch in an options hierarchy.                                   |
| [`MPCLI006`](/ModularPipelines/docs/next/analyzers/MPCLI006.md) | Usage    | Error            | Public analyzer package     | CLI attributes used outside CommandLineToolOptions.                             |
