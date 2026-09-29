using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LibDECRAFT
{
    public partial class JavaClassReader
    {
        public static JavaClassInfo ReadJavaClassFromStream(Stream input)
        {
            JavaClassInfo ret = new JavaClassInfo();
            ret.magicNumber = Utils.StreamReadInt(input);

            ret.versionMinor = Utils.StreamReadShort(input);
            ret.versionMajor = Utils.StreamReadShort(input);

            short nEntriesConstantPool = Utils.StreamReadShort(input);
            ret.entries = new List<ConstantPoolEntry>();
            ret.entries.Add(new ConstantPoolEntry());
            for (int x = 1; x < nEntriesConstantPool; x++)
            {
                int index = input.ReadByte();
                if (index < ConstantPoolEntry.idMap.Length && ConstantPoolEntry.idMap[index] != null)
                {
                    ConstantPoolEntry newCPoolEntry = ConstantPoolEntry.idMap[index].Parse(input);
                    //Console.WriteLine($"[{x}/{nEntriesConstantPool}] Adding new " + newCPoolEntry.GetType().Name + (newCPoolEntry is ConstantPoolEntry.StringEntry ? ": " + ((ConstantPoolEntry.StringEntry)newCPoolEntry).value : ""));
                    if (newCPoolEntry is ConstantPoolEntry.DoubleEntry || newCPoolEntry is ConstantPoolEntry.LongEntry)
                    {
                        //i honestly have no idea why this works
                        ret.entries.Add(new ConstantPoolEntry());
                        x++;
                    }
                    ret.entries.Add(newCPoolEntry);
                } else
                {
                    Console.WriteLine($"!! INVALID ConstPool ID: {index}");
                }
            }

            ret.classAccessFlags = Utils.StreamReadShort(input);
            ret.thisClassNameIndex = Utils.StreamReadShort(input);
            ret.superClassNameIndex = Utils.StreamReadShort(input);
            //Console.WriteLine(((ConstantPoolEntry.ClassReferenceEntry)ret.entries[ret.thisClassNameIndex]).GetName(ret.entries));
            //Console.WriteLine(((ConstantPoolEntry.ClassReferenceEntry)ret.entries[ret.superClassNameIndex]).GetName(ret.entries));
            //ConstantPoolEntry.ClassReferenceEntry superClassRef = (ConstantPoolEntry.ClassReferenceEntry)ret.entries[ret.superClassNameIndex];
            //Console.WriteLine("This class: " + ((ConstantPoolEntry.StringEntry)ret.entries[thisClassRef.indexOfClassNameString]).value);
            //Console.WriteLine("Superclass: " + ((ConstantPoolEntry.StringEntry)ret.entries[superClassRef.indexOfClassNameString]).value);
            

            short nEntriesInterfacesTable = Utils.StreamReadShort(input);
            List<short> interfaceTable = new List<short>();
            for (int x = 0; x < nEntriesInterfacesTable; x++)
            {
                interfaceTable.Add(Utils.StreamReadShort(input));
            }

            short nEntriesFieldTable = Utils.StreamReadShort(input);
            ret.fields = new List<JavaFieldInfo>();
            for (int x = 0; x < nEntriesFieldTable; x++)
            {
                JavaFieldInfo newField = new JavaFieldInfo();
                newField.accessFlags = Utils.StreamReadShort(input);
                newField.nameIndex = Utils.StreamReadShort(input);
                newField.descriptorIndex = Utils.StreamReadShort(input);
                short attributes_count = Utils.StreamReadShort(input);
                for (int y = 0; y < attributes_count; y++)
                {
                    //don't care about any of this
                    Utils.StreamReadShort(input);   //attribute name index
                    int attribute_length = Utils.StreamReadInt(input);
                    for (int z = 0; z < attribute_length; z++)
                    {
                        input.ReadByte();   //info element
                    }
                }
                ret.fields.Add(newField);
            }

            short nEntriesMethodTable = Utils.StreamReadShort(input);
            ret.methods = new List<JavaMethodInfo>();
            for (int x = 0; x < nEntriesMethodTable; x++)
            {
                JavaMethodInfo newMethod = new JavaMethodInfo();
                newMethod.accessFlags = Utils.StreamReadShort(input);
                newMethod.nameIndex = Utils.StreamReadShort(input);
                newMethod.descriptorIndex = Utils.StreamReadShort(input);
                short attributes_count = Utils.StreamReadShort(input);
                for (int y = 0; y < attributes_count; y++)
                {
                    //don't care
                    Utils.StreamReadShort(input);   //attribute name index
                    int attribute_length = Utils.StreamReadInt(input);
                    for (int z = 0; z < attribute_length; z++)
                    {
                        input.ReadByte();   //info element
                    }
                }
                ret.methods.Add(newMethod);
            }

            /*foreach (JavaMethodInfo method in ret.methods)
            {
                Console.WriteLine(method.GetNameAndDescriptor(ret.entries));
            }*/

            //Console.WriteLine("Parse finished");
            return ret;
        }

        public static JavaClassInfo ReadJavaClassFromFile(string path)
        {
            using (FileStream f = File.OpenRead(path))
            {
                return ReadJavaClassFromStream(f);
            }
        }
    }
}
