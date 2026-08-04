### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
MP0001 | Usage | Error | Accesses a module result without declaring the dependency
MP0002 | Usage | Error | Uses IEnumerable as a module result instead of a concrete collection
MP0003 | Usage | Error | Injects a logger into a module constructor instead of using the module context
MP0004 | Usage | Error | Writes directly to System.Console instead of using pipeline logging
MP0005 | Usage | Error | Creates a circular dependency between modules
MP0006 | Usage | Error | Implements ExecuteAsync without the async modifier
MP0007 | Usage | Error | Awaits the current module instance
MP0008 | Design | Warning | Declares mutable instance state that can leak between module executions
MP0009 | Usage | Error | References a dependency type that does not implement IModule
MP0010 | Usage | Error | Declares a module dependency on itself
MP0013 | Usage | Warning | Module is not registered with the pipeline
MP0014 | Usage | Warning | Async void method in a module
MP0015 | Usage | Warning | Blocking call in ExecuteAsync
MP0016 | Usage | Warning | ExecuteAsync cancellation token is not flowed
MP0017 | Usage | Warning | Thread.Sleep in ExecuteAsync
MP0018 | Usage | Warning | Module class is not public
MP0019 | Usage | Warning | Duplicate DependsOn declaration
MPDEP002 | Usage | Error | Circular dependency detected between modules (renamed from ConflictingDependsOnAttribute)
MPCLI001 | Usage | Error | CliFlag property must be bool? or int?
MPCLI002 | Usage | Error | Value-less bool? CliOption should use CliFlag
MPCLI003 | Usage | Error | Multiple CLI attributes applied to one property
MPCLI004 | Usage | Error | Duplicate CLI switch in an options hierarchy
MPCLI006 | Usage | Error | CLI attributes used outside CommandLineToolOptions

### Removed Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
MissingDependsOnAttribute | Usage | Error | Renamed to MP0001
EnumerableModuleResult | Usage | Error | Renamed to MP0002
LoggerInConstructor | Usage | Error | Renamed to MP0003
ConsoleUse | Usage | Error | Renamed to MP0004
ConflictingDependsOnAttribute | Usage | Error | Renamed to MP0005
AsyncModule | Usage | Error | Renamed to MP0006
AwaitThis | Usage | Error | Renamed to MP0007
StatefulModule | Design | Warning | Renamed to MP0008
MPDEP001 | Usage | Error | Renamed to MP0009
MPDEP003 | Usage | Error | Renamed to MP0010
