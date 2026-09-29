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
            Assert.Multiple(() =>
            {
                Assert.That(cl.ThisClassName, Is.EqualTo("pl/cntrpl/TestClass"));
                Assert.That(cl.SuperClassName, Is.EqualTo("java/lang/Object"));
            });
        }

        [Test]
        public void TestFindMethods()
        {
            Assert.Multiple(() =>
            {
                Assert.That(cl.methods.Any(x=>x.Name == "MethodA"), 
                    "Find MethodA");
                Assert.That(cl.methods.Any(x=>x.Name == "MethodB" && x.Descriptor == "(II)Z"), 
                    "Find MethodB and check its descriptor");
            });
        }

        [Test]
        public void TestFindFields()
        {
            Assert.Multiple(() =>
            {
                Assert.That(cl.fields.Any(x => x.Name == "intField" && x.Descriptor == "I"),
                    "Find intField");
                Assert.That(cl.fields.Any(x => x.Name== "boolField" && x.Descriptor == "Z"),
                    "Find boolField");
                Assert.That(cl.fields.Any(x => x.Name == "stringField" && x.Descriptor == "Ljava/lang/String;"),
                    "Find stringField");
            });
        }
    }
}
