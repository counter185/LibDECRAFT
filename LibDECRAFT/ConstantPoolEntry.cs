using System.Collections.Generic;
using System.IO;

namespace LibDECRAFT
{
    public partial class JavaClassReader
    {
        public class ConstantPoolEntry
        {
            public int tag;

            public static ConstantPoolEntry[] idMap = new ConstantPoolEntry[] {
                null,
                new StringEntry(),
                null,
                new IntegerEntry(),
                new FloatEntry(),
                new LongEntry(),
                new DoubleEntry(),
                new ClassReferenceEntry(),
                new StringReferenceEntry(),
                new FieldReferenceEntry(),
                new MethodReferenceEntry(),
                new InterfaceMethodReferenceEntry(),
                new NameAndTypeDescriptorEntry(),
                null,
                null,
                new MethodHandleEntry(),
                new MethodTypeEntry(),
                new DynamicEntry(),
                new InvokeDynamicEntry(),
                new ModuleEntry(),
                new PackageEntry()
            };

            public virtual ConstantPoolEntry Parse(Stream target)
            {
                return null;
            }

            public class StringEntry : ConstantPoolEntry { 
                public new int tag = 1; 
                public string value = ""; 
                public override ConstantPoolEntry Parse(Stream target)
                {
                    StringEntry newEntry = new StringEntry();
                    short length = Utils.StreamReadShort(target);
                    for (int x = 0; x < length; x++)
                    {
                        newEntry.value += (char)target.ReadByte();
                    }
                    return newEntry;
                }
            }
            public class IntegerEntry : ConstantPoolEntry {
                public new int tag = 3; 
                public int value;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    IntegerEntry newEntry = new IntegerEntry();
                    newEntry.value = Utils.StreamReadInt(target);
                    return newEntry;
                }
            }
            public class FloatEntry : ConstantPoolEntry {
                public new int tag = 4; 
                public float value;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    FloatEntry newEntry = new FloatEntry();
                    //don't care
                    newEntry.value = Utils.StreamReadInt(target);
                    return newEntry;
                }
            }
            public class LongEntry : ConstantPoolEntry {
                public new int tag = 5; 
                public long value;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    LongEntry newEntry = new LongEntry();
                    newEntry.value = Utils.StreamReadLong(target);
                    return newEntry;
                }
            }
            public class DoubleEntry : ConstantPoolEntry {
                public new int tag = 6; 
                public double value;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    DoubleEntry newEntry = new DoubleEntry();
                    //don't care
                    newEntry.value = Utils.StreamReadLong(target);
                    return newEntry;
                }
            }
            public class ClassReferenceEntry : ConstantPoolEntry {
                public new int tag = 7; 
                public int indexOfClassNameString;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    ClassReferenceEntry newEntry = new ClassReferenceEntry();
                    newEntry.indexOfClassNameString = Utils.StreamReadShort(target);
                    return newEntry;
                }
                public string GetName(List<ConstantPoolEntry> constantPool)
                {
                    return ((StringEntry)constantPool[indexOfClassNameString]).value;
                }
            }
            public class StringReferenceEntry : ConstantPoolEntry { 
                public new int tag = 8; 
                public int indexOfTargetString;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    StringReferenceEntry newEntry = new StringReferenceEntry();
                    newEntry.indexOfTargetString = Utils.StreamReadShort(target);
                    return newEntry;
                }
            }
            public class FieldReferenceEntry : ConstantPoolEntry {
                public new int tag = 9; 
                public int indexOfClassReference; 
                public int indexOfNameAndTypeDescriptor;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    FieldReferenceEntry newEntry = new FieldReferenceEntry();
                    newEntry.indexOfClassReference = Utils.StreamReadShort(target);
                    newEntry.indexOfNameAndTypeDescriptor = Utils.StreamReadShort(target);
                    return newEntry;
                }
            }
            public class MethodReferenceEntry : ConstantPoolEntry { 
                public new int tag = 10; 
                public int indexOfClassReference; 
                public int indexOfNameAndTypeDescriptor;

                public string ClassReferenceName(List<ConstantPoolEntry> constantPool) => ((ClassReferenceEntry)constantPool[indexOfClassReference]).GetName(constantPool);

                public string Name(List<ConstantPoolEntry> constantPool)
                {
                    NameAndTypeDescriptorEntry nameAndTypeDescriptor = (NameAndTypeDescriptorEntry)constantPool[indexOfNameAndTypeDescriptor];
                    return ((StringEntry)constantPool[nameAndTypeDescriptor.indexOfNameString]).value;
                }
                public string Descriptor(List<ConstantPoolEntry> constantPool)
                {
                    NameAndTypeDescriptorEntry nameAndTypeDescriptor = (NameAndTypeDescriptorEntry)constantPool[indexOfNameAndTypeDescriptor];
                    return ((StringEntry)constantPool[nameAndTypeDescriptor.indexOfTypeDescriptor]).value;
                }

                public string NameAndDescriptor(List<ConstantPoolEntry> constantPool)
                {
                    return Name(constantPool) + Descriptor(constantPool);
                }

                public override ConstantPoolEntry Parse(Stream target)
                {
                    MethodReferenceEntry newEntry = new MethodReferenceEntry();
                    newEntry.indexOfClassReference = Utils.StreamReadShort(target);
                    newEntry.indexOfNameAndTypeDescriptor = Utils.StreamReadShort(target);
                    return newEntry;
                }
            }
            public class InterfaceMethodReferenceEntry : ConstantPoolEntry { 
                public new int tag = 11; 
                public int indexOfClassReference; 
                public int indexOfNameAndTypeDescriptor;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    InterfaceMethodReferenceEntry newEntry = new InterfaceMethodReferenceEntry();
                    newEntry.indexOfClassReference = Utils.StreamReadShort(target);
                    newEntry.indexOfNameAndTypeDescriptor = Utils.StreamReadShort(target);
                    return newEntry;
                }
            }
            public class NameAndTypeDescriptorEntry : ConstantPoolEntry {
                public new int tag = 12; 
                public int indexOfNameString; 
                public int indexOfTypeDescriptor;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    NameAndTypeDescriptorEntry newEntry = new NameAndTypeDescriptorEntry();
                    newEntry.indexOfNameString = Utils.StreamReadShort(target);
                    newEntry.indexOfTypeDescriptor = Utils.StreamReadShort(target);
                    return newEntry;
                }
            }
            public class MethodHandleEntry : ConstantPoolEntry { 
                public new int tag = 15; 
                byte typeDescriptor; 
                int indexOfMethod;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    MethodHandleEntry newEntry = new MethodHandleEntry();
                    newEntry.typeDescriptor = (byte)target.ReadByte();
                    newEntry.indexOfMethod = Utils.StreamReadShort(target);
                    return newEntry;
                }
            }
            public class MethodTypeEntry : ConstantPoolEntry {
                public new int tag = 16; 
                int indexOf;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    MethodTypeEntry newEntry = new MethodTypeEntry();
                    newEntry.indexOf = Utils.StreamReadShort(target);
                    return newEntry;
                }
            }
            public class DynamicEntry : ConstantPoolEntry {
                public new int tag = 17; 
                int data;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    DynamicEntry newEntry = new DynamicEntry();
                    newEntry.data = Utils.StreamReadInt(target);
                    return newEntry;
                }
            }
            public class InvokeDynamicEntry : ConstantPoolEntry {
                public new int tag = 18; 
                int data;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    InvokeDynamicEntry newEntry = new InvokeDynamicEntry();
                    newEntry.data = Utils.StreamReadInt(target);
                    return newEntry;
                }
            }
            public class ModuleEntry : ConstantPoolEntry { 
                public new int tag = 19; 
                int id;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    ModuleEntry newEntry = new ModuleEntry();
                    newEntry.id = Utils.StreamReadShort(target);
                    return newEntry;
                }
            }
            public class PackageEntry : ConstantPoolEntry {
                public new int tag = 20; 
                int id;
                public override ConstantPoolEntry Parse(Stream target)
                {
                    PackageEntry newEntry = new PackageEntry();
                    newEntry.id = Utils.StreamReadShort(target);
                    return newEntry;
                }
            }
        }
    }
}
