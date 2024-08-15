---
title: Execution and Dependencies
sidebar_position: 3
---

# Execution and Dependencies

The default behaviour is for modules to run in parallel, to speed up a pipeline as much as possible. 

If you don't want a particular module to start until another one has finished, then you simply add a `[DependsOn<TModule>]` attribute to your module class.

These can chain together as appropriate. And it'll detect if two modules depend on each other.

```csharp
[DependsOn<Module1>] // or [DependsOn(typeof(Module1))] for older language versions
public class Module2 : Module
{
    ...
}
```