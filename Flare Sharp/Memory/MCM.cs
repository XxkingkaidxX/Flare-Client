﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Flare_Sharp.Memory
{
    /*
     * My awesome memory lib i used in FAKE
     * - ASM
     */
    public class MCM
    {
        [DllImport("kernel32", SetLastError = true)]
        public static extern int ReadProcessMemory(IntPtr hProcess, UInt64 lpBase, ref Int64 lpBuffer, int nSize, int lpNumberOfBytesRead);
        [DllImport("kernel32", SetLastError = true)]
        public static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref byte lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", SetLastError = true)]
        public static extern int VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, Int64 flNewProtect, ref Int64 lpflOldProtect
        );
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        public static IntPtr mcProcHandle;
        public static ProcessModule mcMainModule;
        public static IntPtr mcBaseAddress;

        public static void openGame()
        {
            Process[] procs = Process.GetProcessesByName("Minecraft.Windows");
            Process mcw10 = procs[0];
            IntPtr proc = OpenProcess(0x1F0FFF, false, mcw10.Id);
            mcProcHandle = proc;
            mcMainModule = mcw10.MainModule;
            mcBaseAddress = mcMainModule.BaseAddress;
        }

        public static void unprotectMemory(IntPtr address, int bytesToUnprotect)
        {
            Int64 receiver = 0;
            VirtualProtectEx(mcProcHandle, address, bytesToUnprotect, 0x40, ref receiver);
        }

        //CE bytes to real bytes for ease
        public static byte[] ceByte2Bytes(string byteString)
        {
            string[] byteStr = byteString.Split(' ');
            byte[] bytes = new byte[byteStr.Length];
            int c = 0;
            foreach (string b in byteStr)
            {
                bytes[c] = (Convert.ToByte(b, 16));
                c++;
            }
            return bytes;
        }
        public static int[] ceByte2Ints(string byteString)
        {
            string[] intStr = byteString.Split(' ');
            int[] ints = new int[intStr.Length];
            int c = 0;
            foreach (string b in intStr)
            {
                ints[c] = (int.Parse(b, System.Globalization.NumberStyles.HexNumber));
                c++;
            }
            return ints;
        }

        public static Int64 evaluatePointer(int offset, int[] offsets)
        {
            Int64 buffer = 0;
            ReadProcessMemory(mcProcHandle, (UInt64)(mcBaseAddress + offset), ref buffer, sizeof(UInt64), 0);
            for (int i = 0; i < offsets.Length - 1; i++)
            {
                ReadProcessMemory(mcProcHandle, (UInt64)(buffer + offsets[i]), ref buffer, sizeof(UInt64), 0);
            }
            return buffer + offsets[offsets.Length - 1];
        }

        //Read base
        public static int readBaseByte(int offset)
        {
            Int64 buffer = 0;
            ReadProcessMemory(mcProcHandle, (UInt64)(mcBaseAddress + offset), ref buffer, sizeof(byte), 0);
            return (byte)buffer;
        }
        public static int readBaseInt(int offset)
        {
            Int64 buffer = 0;
            ReadProcessMemory(mcProcHandle, (UInt64)(mcBaseAddress + offset), ref buffer, sizeof(int), 0);
            return (int)buffer;
        }
        public static Int64 readBaseInt64(int offset)
        {
            Int64 buffer = 0;
            ReadProcessMemory(mcProcHandle, (UInt64)(mcBaseAddress + offset), ref buffer, sizeof(Int64), 0);
            return buffer;
        }

        //Write base
        public static void writeBaseByte(int offset, byte value)
        {
            WriteProcessMemory(mcProcHandle, (mcBaseAddress + offset), ref value, sizeof(byte), 0);
        }
        public static void writeBaseInt(int offset, int value)
        {
            byte[] intByte = BitConverter.GetBytes(value);
            int inc = 0;
            foreach (byte b in intByte)
            {
                writeBaseByte(offset + inc, b);
                inc++;
            }
        }
        public static void writeBaseBytes(int offset, byte[] value)
        {
            int inc = 0;
            foreach (byte b in value)
            {
                writeBaseByte(offset + inc, b);
                inc++;
            }
        }
        public static void writeBaseFloat(int offset, float value)
        {
            byte[] intByte = BitConverter.GetBytes(value);
            int inc = 0;
            foreach (byte b in intByte)
            {
                writeBaseByte(offset + inc, b);
                inc++;
            }
        }

        //Read direct
        public static int readByte(UInt64 address)
        {
            Int64 buffer = 0;
            ReadProcessMemory(mcProcHandle, address, ref buffer, sizeof(byte), 0);
            return (byte)buffer;
        }
        public static int readInt(UInt64 address)
        {
            Int64 buffer = 0;
            ReadProcessMemory(mcProcHandle, address, ref buffer, sizeof(int), 0);
            return (int)buffer;
        }
        public static Int64 readInt64(UInt64 address)
        {
            Int64 buffer = 0;
            ReadProcessMemory(mcProcHandle, address, ref buffer, sizeof(int), 0);
            return buffer;
        }

        //Write direct
        public static void writeByte(Int64 address, byte value)
        {
            WriteProcessMemory(mcProcHandle, (IntPtr)address, ref value, sizeof(byte), 0);
        }
        public static void writeBytes(int address, byte[] value)
        {
            int inc = 0;
            foreach (byte b in value)
            {
                writeByte(address + inc, b);
                inc++;
            }
        }
        public static void writeInt(Int64 address, int value)
        {
            byte[] intByte = BitConverter.GetBytes(value);
            int inc = 0;
            foreach (byte b in intByte)
            {
                writeByte(address + inc, b);
                inc++;
            }
        }
        public static void writeFloat(Int64 address, float value)
        {
            byte[] intByte = BitConverter.GetBytes(value);
            int inc = 0;
            foreach (byte b in intByte)
            {
                writeByte(address + inc, b);
                inc++;
            }
        }
    }
}