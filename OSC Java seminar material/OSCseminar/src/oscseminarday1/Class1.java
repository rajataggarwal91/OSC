/* Program no 5
 * Basic class with attributes and methods
 * Classes objects are like structures but with added functions which can work on them
 Show use of this operator
 *
 */


package oscseminarday1;

public class Class1 {

    private int a = 10; //attribute

    public static void main(String args[]) {
        Class1 obj = new Class1();   //creation of object
        obj.myMethod();         //call of method
    }

    public void myMethod() {
        //a = 20;
        System.out.println("value of a = " + a);
        int a = 100;
        //'this' avoids ambuiguity by considering the present object's attribute
        System.out.println("Showing use of this, a=" + this.a);
        System.out.println("value of a = " + a);
    }
}
