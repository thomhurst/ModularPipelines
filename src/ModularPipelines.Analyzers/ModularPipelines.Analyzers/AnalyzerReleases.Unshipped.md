### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
AwaitThis | Usage | Error | Prohibit await this in modules
StatefulModule | Design | Warning | Detects mutable instance fields in modules that could leak state between executions
MP0010 | Migration | Warning | Warns when deprecated Arguments property is used on CommandLineToolOptions
MPDEP001 | Usage | Error | DependsOn type does not implement IModule
MPDEP002 | Usage | Error | Circular dependency detected between modules (renamed from ConflictingDependsOnAttribute)
MPDEP003 | Usage | Error | Module depends on itself

### Removed Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
ConflictingDependsOnAttribute | Usage | Error | Renamed to MPDEP002
