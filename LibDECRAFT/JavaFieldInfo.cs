using System.Collections.Generic;

namespace LibDECRAFT
{
    public partial class JavaClassReader
    {
        public class JavaFieldInfo
        {
            public short accessFlags;
            public short nameIndex;
            public short descriptorIndex;

            public bool IsPublic => (accessFlags & 0x0001) != 0;
            public bool IsStatic => (accessFlags & 0x0008) != 0;

            public string Descriptor(List<ConstantPoolEntry> constantPool) => ((ConstantPoolEntry.StringEntry)constantPool[descriptorIndex]).value;

            public string Name(List<ConstantPoolEntry> constantPool) => ((ConstantPoolEntry.StringEntry)constantPool[nameIndex]).value;
        }
    }
}
