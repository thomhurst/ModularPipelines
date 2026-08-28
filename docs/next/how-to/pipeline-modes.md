# Pipeline Failure Modes

A pipeline has two failure modes:

* FailFast
* ContinueOnFailure

By default, a pipeline uses `FailFast`. As soon as any module throws an exception, the pipeline fails and terminates for fast feedback.

Use `ContinueOnFailure` to let independent modules finish before the pipeline evaluates failures. Modules whose required dependencies fail still do not run.

## Example[​](#example "Direct link to Example")

```
var builder = Pipeline.CreateBuilder(args);



builder

    .AddModule<Module1>()

    .AddModule<Module2>()

    .AddModule<Module3>();



builder.ConfigurePipelineOptions(options => options with

{

    FailureMode = FailureMode.ContinueOnFailure,

});



await builder.RunAsync();
```
