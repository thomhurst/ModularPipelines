---
title: Cancellation Tokens
---

# Cancellation Tokens

When you override a Module's `ExecuteAsync` method, you are provided a `CancellationToken` by the framework.

It is recommended to use this token, and pass it in everywhere applicable. This token will be cancelled if the pipeline fails for any reason, and it'll help cancel any pending operations that haven't yet completed.

## Example

```csharp
public class MyModule : Module<File>
{
    protected override async Task<File?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return await context.Downloader.DownloadFileAsync(new DownloadFileOptions(new Uri("https://www.example.com/somefile.zip")), cancellationToken);
    }
}
```