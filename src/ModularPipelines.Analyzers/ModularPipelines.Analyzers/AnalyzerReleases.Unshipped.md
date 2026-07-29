### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
AwaitThis | Usage | Error | Prohibit await this in modules
StatefulModule | Design | Warning | Detects mutable instance fields in modules that could leak state between executions
MPDEP001 | Usage | Error | DependsOn type does not implement IModule
MPDEP002 | Usage | Error | Circular dependency detected between modules (renamed from ConflictingDependsOnAttribute)
MPDEP003 | Usage | Error | Module depends on itself
MPREG001 | Usage | Warning | Module is not registered with the pipeline
MPASYNC001 | Usage | Error | Async void method in a module
MPASYNC002 | Usage | Warning | Blocking call in ExecuteAsync
MPASYNC003 | Usage | Warning | ExecuteAsync cancellation token is not flowed
MPASYNC004 | Usage | Warning | Thread.Sleep in ExecuteAsync
MPTYPE001 | Usage | Warning | Module class is not public
MPDEP004 | Usage | Error | Duplicate DependsOn declaration

### Removed Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
ConflictingDependsOnAttribute | Usage | Error | Renamed to MPDEP002
