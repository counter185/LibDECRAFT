using System.Collections.Generic;

namespace LibDECRAFT
{
    public partial class JavaClassReader
    {
        public class JavaClassInfo
        {
            public int magicNumber;
            public short versionMinor;
            public short versionMajor;
            public short classAccessFlags;
            public short thisClassNameIndex;
            public short superClassNameIndex;
            public List<ConstantPoolEntry> entries;
            public List<JavaMethodInfo> methods;
            public List<JavaFieldInfo> fields;

            public string ThisClassName(List<ConstantPoolEntry> cpool) => (cpool[thisClassNameIndex] is ConstantPoolEntry.ClassReferenceEntry) ? ((ConstantPoolEntry.ClassReferenceEntry)cpool[thisClassNameIndex]).GetName(cpool) : "<invalid>";
            public string SuperClassName(List<ConstantPoolEntry> cpool) => (cpool[superClassNameIndex] is ConstantPoolEntry.ClassReferenceEntry) ? ((ConstantPoolEntry.ClassReferenceEntry)cpool[superClassNameIndex]).GetName(cpool) : "<invalid>";
        }
    }
}
