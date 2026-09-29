# LibDECRAFT

.NET library for parsing Java class files, adapted from code used in the universal Java program launcher [DECRAFT](https://github.com/counter185/DECRAFT_Launcher).

# Usage

## Parse a class from file

```csharp
JavaClassInfo javaClass = JavaClassReader.ReadJavaClassFromFile("SampleClass.class");
```

## Parse a class from stream (such as zip)

```csharp
using (ZipArchive archive = ZipFile.OpenRead("program.jar")) {
    using (Stream s = archive.GetEntry("SampleClass.class").Open()) {
        JavaClassInfo javaClass = JavaClassReader.ReadJavaClassFromStream(s);
    }
}
```

## Get information about a class
```csharp
Console.WriteLine(javaClass.versionMajor);      // -> 61
Console.WriteLine(javaClass.versionMinor);      // -> 0

Console.WriteLine(javaClass.ThisClassName);     // -> com/example/SampleClass
Console.WriteLine(javaClass.SuperClassName);    // -> java/lang/Object
```

## Print information about methods
```csharp
foreach (JavaMethodInfo method in javaClass.methods) {
    Console.WriteLine($"{method.Name} | {method.Descriptor}");
}

// Sample output:
//    <init> | ()V
//    MethodA | ()V
//    MethodB | (II)Z
```

## Print information about fields
```csharp
foreach (JavaFieldInfo field in javaClass.fields)
{
    Console.WriteLine($"{field.Name} | {field.Descriptor}");
}

// Sample output:
//    intField | I
//    boolField | Z
//    stringField | Ljava/lang/String;
//    doubleField | D
```

## Get constant pool entries
```csharp
foreach (ConstantPoolEntry entry in javaClass.entries)
{
    Console.WriteLine($"{entry.Tag()} ({entry.GetType()})");
}

// Sample output:
//    0 (LibDECRAFT.ConstantPoolEntry)
//    10 (LibDECRAFT.ConstantPoolEntry+MethodReferenceEntry)
//    7 (LibDECRAFT.ConstantPoolEntry+ClassReferenceEntry)
//    12 (LibDECRAFT.ConstantPoolEntry+NameAndTypeDescriptorEntry)
//    1 (LibDECRAFT.ConstantPoolEntry+StringEntry)
//    1 (LibDECRAFT.ConstantPoolEntry+StringEntry)
//    1 (LibDECRAFT.ConstantPoolEntry+StringEntry)
//    9 (LibDECRAFT.ConstantPoolEntry+FieldReferenceEntry)
//    ...

Console.WriteLine(((ConstantPoolEntry.StringEntry)cl.entries[4]).value);
// -> java/lang/Object
```