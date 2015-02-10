using System;
using System.Runtime.InteropServices;

namespace Sleipnir
{
    class WTSapi32
    {
        // Import the channel open function from wtsapi32
        [DllImport("Wtsapi32.dll")]
        public static extern IntPtr WTSVirtualChannelOpen(
            IntPtr server,
            int sessionId,
            [MarshalAs(UnmanagedType.LPStr)] string virtualName);

        // Import the channel query function from wtsapi32
        [DllImport("Wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSVirtualChannelQuery(
            IntPtr channelHandle, VirtualClass virtualClass,
            out IntPtr buffer,
            ref uint bytesReturned);

        //import the channel read function from wtsapi32
        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern int WTSVirtualChannelRead(IntPtr hChannel,
        uint Timeout,
        [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] Buffer,
        uint BufferSize,
        out uint BytesRead);


        // Import the free memory function from wtsapi32
        [DllImport("Wtsapi32.dll", SetLastError = true)]
        public static extern void WTSFreeMemory(
            IntPtr memory);

        // Import the channel write function from wtsapi32
        [DllImport("Wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSVirtualChannelWrite(
            IntPtr channelHandle,
            byte[] data,
            int length,
            ref int bytesWritten);

        // Import the channel close function from wtsapi32
        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSVirtualChannelClose(IntPtr channelHandle);


        public enum VirtualClass
        {
            ClientData,
            FileHandle
        };

        

    }
}
