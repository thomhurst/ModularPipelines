using Pipeline.NET;
using Pipeline.NET.Build;

var modules = await PipelineHostBuilder.Create()
    .AddModule<RunUnitTestsCommandModule>()
    .ExecutePipelineAsync();