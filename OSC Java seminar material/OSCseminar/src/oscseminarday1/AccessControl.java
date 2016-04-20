package oscseminarday1;

public class AccessControl {

    private int var1;
    protected int var2;
    public int var3;
    int var4;   //default

    public void meth() {
    var1= 4;  //legal
    System.out.println("var1 = "+var1);
    }
}

class MainClass {

    public static void main(String args[]) {
        AccessControl obj = new AccessControl();
        //obj.var1 = 1;   //illegal
        obj.var2 = 2;   //same package and thus available
        obj.var3 = 3;   //public is available
        obj.var4 = 4;   //default is available in same package
        obj.meth();     //public is available
    }
}

