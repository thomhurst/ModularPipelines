# Java Package

`ModularPipelines.Java` provides strongly typed access to Maven and Gradle build CLIs.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Java
```

The `mvn` or `gradle` executable must be installed and available on `PATH` when its service runs.

## Maven[​](#maven "Direct link to Maven")

```
using ModularPipelines.Java.Enums;

using ModularPipelines.Java.Options;

using ModularPipelines.Models;



var result = await context.Tools.Maven.ExecuteAsync(

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

```
mvn --batch-mode --color never --define skipTests=true clean verify
```

## Gradle[​](#gradle "Direct link to Gradle")

```
var result = await context.Tools.Gradle.ExecuteAsync(

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

```
gradle --console plain --project-prop environment=ci --max-workers 4 --no-daemon clean build
```

Generated option records include current CLI aliases, repeatable properties, constrained enums, and secret annotations. Maven goals/phases and Gradle tasks are emitted after options.
