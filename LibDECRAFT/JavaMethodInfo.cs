using System;
using System.Collections.Generic;
using System.Linq;

namespace LibDECRAFT
{
    public class JavaMethodInfo
    {
        public JavaClassInfo parentClass;

        public short accessFlags;
        public short nameIndex;
        public short descriptorIndex;

        public static readonly Dictionary<int, string> accessFlagNames = new Dictionary<int, string>()
        {
            { 0x0001, "public" },
            { 0x0002, "private" },
            { 0x0004, "protected" },
            { 0x0008, "static" },
            { 0x0010, "final" },
            { 0x0020, "synchronized" },
            { 0x0100, "native" },
            { 0x0400, "abstract" },
        };

        public bool IsPublic => (accessFlags & 0x0001) != 0;
        public bool IsStatic => (accessFlags & 0x0008) != 0;

        public string Name => ((ConstantPoolEntry.StringEntry)parentClass.entries[nameIndex]).value;
        public string Descriptor => ((ConstantPoolEntry.StringEntry)parentClass.entries[descriptorIndex]).value;

    }
}
