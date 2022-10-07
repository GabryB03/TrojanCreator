using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.ComponentModel;
using System.Security.Principal;
using System;

public class CDACL
{
    public static void ChangeDACLStatus(IntPtr ptrProc, bool bProtect)
    {
        var DACL = GetProcessSecurityDescriptor(ptrProc);

        switch (bProtect)
        {
            case true:
                DACL.DiscretionaryAcl.InsertAce(0,
                new CommonAce(AceFlags.None, AceQualifier.AccessDenied,
                (int)ProcessAccessRights.PROCESS_ALL_ACCESS,
                new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                false, null));
                break;

            case false:
                DACL.DiscretionaryAcl.InsertAce(0,
                new CommonAce(AceFlags.None, AceQualifier.AccessDenied,
                (int)ProcessAccessRights.PROCESS_ALL_ACCESS,
                new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                false, null));

                break;
        }

        SetProcessSecurityDescriptor(ptrProc, DACL);
    }


    [DllImport("advapi32.dll", SetLastError = true)]
    static extern bool GetKernelObjectSecurity(IntPtr Handle, int securityInformation, [Out] byte[] pSecurityDescriptor,
    uint nLength, out uint lpnLengthNeeded);

    private static RawSecurityDescriptor GetProcessSecurityDescriptor(IntPtr processHandle)
    {
        const int DACL_SECURITY_INFORMATION = 0x00000004;
        byte[] psd = new byte[0];
        uint bufSizeNeeded;
        GetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION, psd, 0, out bufSizeNeeded);

        if (bufSizeNeeded < 0 || bufSizeNeeded > short.MaxValue)
        {
            throw new Win32Exception();
        }

        if (!GetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION, psd = new byte[bufSizeNeeded], bufSizeNeeded, out bufSizeNeeded))
        {
            throw new Win32Exception();
        }

        return new RawSecurityDescriptor(psd, 0);
    }

    [DllImport("advapi32.dll", SetLastError = true)]
    static extern bool SetKernelObjectSecurity(IntPtr Handle, int securityInformation, [In] byte[] pSecurityDescriptor);

    private static void SetProcessSecurityDescriptor(IntPtr processHandle, RawSecurityDescriptor dacl)
    {
        const int DACL_SECURITY_INFORMATION = 0x00000004;
        byte[] rawsd = new byte[dacl.BinaryLength];
        dacl.GetBinaryForm(rawsd, 0);

        if (!SetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION, rawsd))
        {
            throw new Win32Exception();
        }
    }

    [Flags]
    private enum ProcessAccessRights
    {
        PROCESS_CREATE_PROCESS = 0x0080,
        PROCESS_CREATE_THREAD = 0x0002,
        PROCESS_DUP_HANDLE = 0x0040,
        PROCESS_QUERY_INFORMATION = 0x0400,
        PROCESS_QUERY_LIMITED_INFORMATION = 0x1000,
        PROCESS_SET_INFORMATION = 0x0200,
        PROCESS_SET_QUOTA = 0x0100,
        PROCESS_SUSPEND_RESUME = 0x0800,
        PROCESS_TERMINATE = 0x0001,
        PROCESS_VM_OPERATION = 0x0008,
        PROCESS_VM_READ = 0x0010,
        PROCESS_VM_WRITE = 0x0020,
        DELETE = 0x00010000,
        READ_CONTROL = 0x00020000,
        SYNCHRONIZE = 0x00100000,
        WRITE_DAC = 0x00040000,
        WRITE_OWNER = 0x00080000,
        STANDARD_RIGHTS_REQUIRED = 0x000f0000,
        PROCESS_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF)
    }
}