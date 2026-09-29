using static LibDECRAFT.JavaClassReader;

namespace LibDECRAFT.Test
{
    public class Tests
    {
        public JavaClassInfo cl;

        [SetUp]
        public void Setup()
        {
            cl = JavaClassReader.ReadJavaClassFromFile("./Data/TestClass.class");
        }

        [Test]
        public void TestPackageName()
        {
            Assert.That(cl.ThisClassName(cl.entries), Is.EqualTo("pl/cntrpl/TestClass"));
        }

        [Test]
        public void TestFindMethods()
        {
            Assert.That(cl.methods.Any(x=>x.Name(cl.entries) == "MethodA"), 
                "Find MethodA");
            //Console.WriteLine(cl.methods.Where(x => x.Name(cl.entries) == "MethodB").First().Descriptor(cl.entries));
            Assert.That(cl.methods.Any(x=>x.Name(cl.entries) == "MethodB" && x.Descriptor(cl.entries) == "(II)Z"), 
                "Find MethodB and check its descriptor");
        }

        [Test]
        public void TestFindFields()
        {
            Assert.That(cl.fields.Any(x => x.Name(cl.entries) == "intField" && x.Descriptor(cl.entries) == "I"),
                "Find intField");
            Assert.That(cl.fields.Any(x => x.Name(cl.entries) == "boolField" && x.Descriptor(cl.entries) == "Z"),
                "Find boolField");
            Assert.That(cl.fields.Any(x => x.Name(cl.entries) == "stringField" && x.Descriptor(cl.entries) == "Ljava/lang/String;"),
                "Find stringField");
        }
    }
}
