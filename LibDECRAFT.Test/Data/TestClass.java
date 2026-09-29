package pl.cntrpl;

public class TestClass {
    public int intField = 2;
    public boolean boolField = true;
    public String stringField = "test string";

    public void MethodA() {
        System.out.println("MethodA()");
    }

    public boolean MethodB(int arg1, int arg2) {
        System.out.println(arg1 + " " + arg2);
        return true;
    }
}