using System.Collections.Generic;

namespace LibDECRAFT
{
    public class JavaFieldInfo
    {
        public JavaClassInfo parentClass;

        public short accessFlags;
        public short nameIndex;
        public short descriptorIndex;

        public bool IsPublic => (accessFlags & 0x0001) != 0;
        public bool IsStatic => (accessFlags & 0x0008) != 0;

        public string Descriptor => ((ConstantPoolEntry.StringEntry)parentClass.entries[descriptorIndex]).value;

        public string Name => ((ConstantPoolEntry.StringEntry)parentClass.entries[nameIndex]).value;
    }
}
