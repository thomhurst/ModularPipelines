
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using Pipeline.NET.Context;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.NuGet;

public abstract class NuGetUploadModule : Module
{
    protected NuGetUploadModule(IModuleContext context) : base(context)
    {
    }
    
    protected abstract IEnumerable<string> PackagePaths { get; }
    protected abstract string AccessToken { get; }
    protected abstract Uri FeedUri { get; }

    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var packageSource = new PackageSource(FeedUri.AbsolutePath)
        {
            Credentials = new PackageSourceCredential(
                source: FeedUri.AbsolutePath,
                username: string.Empty, // not important
                passwordText: AccessToken,
                isPasswordClearText: true,
                validAuthenticationTypesText: null)
        };
        
        var repository = Repository.Factory.GetCoreV3(packageSource);
        
        var resource = await repository.GetResourceAsync<PackageUpdateResource>(cancellationToken);
        
        await resource.Push(
            PackagePaths.ToList(),
            symbolSource: null,
            timeoutInSecond: 5 * 60,
            disableBuffering: false,
            getApiKey: packageSource => string.Empty,
            getSymbolApiKey: packageSource => null,
            noServiceEndpoint: false,
            skipDuplicate: true,
            symbolPackageUpdateResource: null,
            NullLogger.Instance);

        return await NothingAsync();
    }
}