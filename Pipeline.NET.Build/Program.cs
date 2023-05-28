using Pipeline.NET.Build.Modules;
using Pipeline.NET.Host;

var modules = await PipelineHostBuilder.Create()
    .AddModule<RunUnitTestsModule>()
    .AddModule<PublishPackagesModule>()
    .AddModule<UploadPackagesToNugetModule>()
    .AddModule<CleanModule>()
    .AddModule<BinObjFolderRemovalModule>()
    .ExecutePipelineAsync();