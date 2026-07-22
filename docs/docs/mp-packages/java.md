---
title: Java Package
---

# Java Package

`ModularPipelines.Java` provides strongly typed access to Maven and Gradle build CLIs.

## Installation

```shell
dotnet add package ModularPipelines.Java
```

The `mvn` or `gradle` executable must be installed and available on `PATH` when its service runs.

## Maven

```csharp
using ModularPipelines.Java.Enums;
using ModularPipelines.Java.Extensions;
using ModularPipelines.Java.Options;
using ModularPipelines.Models;

var result = await context.Maven().Execute(
    new MavenExecuteOptions
    {
        BatchMode = true,
        Color = MavenColor.Never,
        Define = [new KeyValue("skipTests", "true")],
        GoalsAndPhases = ["clean", "verify"],
    },
    cancellationToken: cancellationToken);
```

This renders:

```shell
mvn --batch-mode --color never --define skipTests=true clean verify
```

## Gradle

```csharp
var result = await context.Gradle().Execute(
    new GradleExecuteOptions
    {
        Console = GradleConsole.Plain,
        ProjectProp = [new KeyValue("environment", "ci")],
        NoDaemon = true,
        MaxWorkers = 4,
        Tasks = ["clean", "build"],
    },
    cancellationToken: cancellationToken);
```

This renders:

```shell
gradle --console plain --project-prop environment=ci --max-workers 4 --no-daemon clean build
```

Generated option records include current CLI aliases, repeatable properties, constrained enums,
and secret annotations. Maven goals/phases and Gradle tasks are emitted after options.
