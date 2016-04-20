/* Program no. 11
 * Demonstrates inheritence
 * Access control in inheritence
 */
package oscseminarday2;

class Base {

    private int i;
    protected int j;
    public int k;

    void showInBase() {
        i = 1;
        System.out.println("i,j and k: " + i + " " + j+" "+k);
    }
}
// Create a subclass by extending class A.
class Derived extends Base {

    int l;

 //the show() function inherited from Base class is overriden
    void showInDerived() {                       //the same signature as in base class
        System.out.println("l: " + l);
        System.out.println("These attributes were inherited and assigned values:");
        System.out.println("j="+j+" "+"k="+k);
        //you also inherites methods from base class
    }
}
class InheritanceDemo {

    public static void main(String args[]) {
    Base superob= new Base();
    superob.j=2;
    superob.k=3;
    superob.showInBase();

    Derived subob= new Derived();
    subob.j=100;
    subob.k=200;
    subob.l=4;
    subob.showInDerived();
    
    }
    }
