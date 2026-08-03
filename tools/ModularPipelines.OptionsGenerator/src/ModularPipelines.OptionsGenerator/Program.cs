using ModularPipelines.OptionsGenerator;
using ModularPipelines.OptionsGenerator.TypeDetection;

if (UnixProcessGroupLauncher.IsInvocation(args))
{
    return await UnixProcessGroupLauncher.RunAsync(args);
}

if (WindowsJobLauncher.IsInvocation(args))
{
    return await WindowsJobLauncher.RunAsync(args);
}

return await OptionsGeneratorCommand.RunAsync(args);
