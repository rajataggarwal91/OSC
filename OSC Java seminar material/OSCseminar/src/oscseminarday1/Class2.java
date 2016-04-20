/* Program no. 6
 * A more practical example of class
 */


package oscseminarday1;

class Class2 {

    double width;
    double height;
    double depth;


    void volume() {
        System.out.print("Volume is ");
        System.out.println(width * height * depth);
    }
}

// main class where execution of program starts
class BoxDemo3 {

    public static void main(String args[]) {
        Class2 mybox1 = new Class2();
        Class2 mybox2 = new Class2();
// assign values to mybox1's instance variables
        mybox1.width = 10;
        mybox1.height = 20;
        mybox1.depth = 15;
        /* assign different values to mybox2's
        instance variables */
        mybox2.width = 3;
        mybox2.height = 6;
        mybox2.depth = 9;
// display volume of first box
        mybox1.volume();
// display volume of second box
        mybox2.volume();
    }
}
