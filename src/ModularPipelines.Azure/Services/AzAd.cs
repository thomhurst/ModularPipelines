using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAd
{
    public AzAd(
        AzAdApp app,
        AzAdDs ds,
        AzAdGroup group,
        AzAdSignedInUser signedInUser,
        AzAdSp sp,
        AzAdUser user
    )
    {
        App = app;
        Ds = ds;
        Group = group;
        SignedInUser = signedInUser;
        Sp = sp;
        User = user;
    }

    public AzAdApp App { get; }

    public AzAdDs Ds { get; }

    public AzAdGroup Group { get; }

    public AzAdSignedInUser SignedInUser { get; }

    public AzAdSp Sp { get; }

    public AzAdUser User { get; }
}