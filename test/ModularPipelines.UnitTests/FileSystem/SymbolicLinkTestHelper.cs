using TUnit.Core.Exceptions;

namespace ModularPipelines.UnitTests.FileSystem;

internal static class SymbolicLinkTestHelper
{
    public static void CreateOrSkip(Action createLink)
    {
        try
        {
            createLink();
        }
        catch (IOException exception) when (OperatingSystem.IsWindows() && exception.HResult == unchecked((int) 0x80070522)) // ERROR_PRIVILEGE_NOT_HELD
        {
            throw new SkipTestException("Symbolic-link creation requires a Windows privilege unavailable to this account (ERROR_PRIVILEGE_NOT_HELD).");
        }
    }
}
