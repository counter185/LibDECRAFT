using System.Collections.Generic;

namespace LibDECRAFT
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

        public string ThisClassName => (entries[thisClassNameIndex] is ConstantPoolEntry.ClassReferenceEntry) ? ((ConstantPoolEntry.ClassReferenceEntry)entries[thisClassNameIndex]).GetName(entries) : "<invalid>";
        public string SuperClassName => (entries[superClassNameIndex] is ConstantPoolEntry.ClassReferenceEntry) ? ((ConstantPoolEntry.ClassReferenceEntry)entries[superClassNameIndex]).GetName(entries) : "<invalid>";
    }
}
