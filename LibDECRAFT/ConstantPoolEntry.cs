using System.Collections.Generic;
using System.IO;

namespace LibDECRAFT
{
    public class ConstantPoolEntry
    {
        public JavaClassInfo parentClass;
        public virtual int Tag() => 0;
        public virtual ConstantPoolEntry Parse(Stream target) => null;

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


        public class StringEntry : ConstantPoolEntry { 
            public override int Tag() => 1; 
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
            public override int Tag() => 3;
            public int value;
            public override ConstantPoolEntry Parse(Stream target)
                => new IntegerEntry { value = Utils.StreamReadInt(target) };
        }
        public class FloatEntry : ConstantPoolEntry {
            public override int Tag() => 4;
            public float value;
            public override ConstantPoolEntry Parse(Stream target)
                => new FloatEntry { value = Utils.StreamReadFloat(target) };
        }
        public class LongEntry : ConstantPoolEntry {
            public override int Tag() => 5;
            public long value;
            public override ConstantPoolEntry Parse(Stream target)
                => new LongEntry { value = Utils.StreamReadLong(target) };
        }
        public class DoubleEntry : ConstantPoolEntry {
            public override int Tag() => 6; 
            public double value;
            public override ConstantPoolEntry Parse(Stream target)
                => new DoubleEntry { value = Utils.StreamReadDouble(target) };
        }
        public class ClassReferenceEntry : ConstantPoolEntry {
            public override int Tag() => 7; 
            public int indexOfClassNameString;
            public override ConstantPoolEntry Parse(Stream target)
                => new ClassReferenceEntry { indexOfClassNameString = Utils.StreamReadShort(target) };

            public string Name => ((StringEntry)parentClass.entries[indexOfClassNameString]).value;
        }
        public class StringReferenceEntry : ConstantPoolEntry {
            public override int Tag() => 8;
            public int indexOfTargetString;
            public override ConstantPoolEntry Parse(Stream target) 
                => new StringReferenceEntry { indexOfTargetString = Utils.StreamReadShort(target) };
        }
        public class FieldReferenceEntry : ConstantPoolEntry {
            public override int Tag() => 9; 
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
            public override int Tag() => 10; 
            public int indexOfClassReference; 
            public int indexOfNameAndTypeDescriptor;

            public string ClassReferenceName 
                => ((ClassReferenceEntry)parentClass.entries[indexOfClassReference]).Name;

            public string Name {
                get {
                    var nameAndTypeDescriptor = (NameAndTypeDescriptorEntry)parentClass.entries[indexOfNameAndTypeDescriptor];
                    return ((StringEntry)parentClass.entries[nameAndTypeDescriptor.indexOfNameString]).value;
                }
            }
            public string Descriptor {
                get {
                    NameAndTypeDescriptorEntry nameAndTypeDescriptor = (NameAndTypeDescriptorEntry)parentClass.entries[indexOfNameAndTypeDescriptor];
                    return ((StringEntry)parentClass.entries[nameAndTypeDescriptor.indexOfTypeDescriptor]).value;
                }
            }

            public string NameAndDescriptor => Name + Descriptor;

            public override ConstantPoolEntry Parse(Stream target)
            {
                MethodReferenceEntry newEntry = new MethodReferenceEntry();
                newEntry.indexOfClassReference = Utils.StreamReadShort(target);
                newEntry.indexOfNameAndTypeDescriptor = Utils.StreamReadShort(target);
                return newEntry;
            }
        }
        public class InterfaceMethodReferenceEntry : ConstantPoolEntry { 
            public override int Tag() => 11; 
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
            public override int Tag() => 12; 
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
            public override int Tag() => 15; 
            public byte typeDescriptor; 
            public int indexOfMethod;
            public override ConstantPoolEntry Parse(Stream target)
            {
                MethodHandleEntry newEntry = new MethodHandleEntry();
                newEntry.typeDescriptor = (byte)target.ReadByte();
                newEntry.indexOfMethod = Utils.StreamReadShort(target);
                return newEntry;
            }
        }
        public class MethodTypeEntry : ConstantPoolEntry {
            public override int Tag() => 16; 
            public int indexOf;
            public override ConstantPoolEntry Parse(Stream target)
            {
                MethodTypeEntry newEntry = new MethodTypeEntry();
                newEntry.indexOf = Utils.StreamReadShort(target);
                return newEntry;
            }
        }
        public class DynamicEntry : ConstantPoolEntry {
            public override int Tag() => 17; 
            public int data;
            public override ConstantPoolEntry Parse(Stream target)
            {
                DynamicEntry newEntry = new DynamicEntry();
                newEntry.data = Utils.StreamReadInt(target);
                return newEntry;
            }
        }
        public class InvokeDynamicEntry : ConstantPoolEntry {
            public override int Tag() => 18; 
            public int data;
            public override ConstantPoolEntry Parse(Stream target)
            {
                InvokeDynamicEntry newEntry = new InvokeDynamicEntry();
                newEntry.data = Utils.StreamReadInt(target);
                return newEntry;
            }
        }
        public class ModuleEntry : ConstantPoolEntry { 
            public override int Tag() => 19; 
            public int id;
            public override ConstantPoolEntry Parse(Stream target)
            {
                ModuleEntry newEntry = new ModuleEntry();
                newEntry.id = Utils.StreamReadShort(target);
                return newEntry;
            }
        }
        public class PackageEntry : ConstantPoolEntry {
            public override int Tag() => 20; 
            public int id;
            public override ConstantPoolEntry Parse(Stream target)
                => new PackageEntry { id = Utils.StreamReadShort(target) };
        }
    }
}
