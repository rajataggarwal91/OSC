/* Program no. 9
 * shows constructor usage and function overloading
 */
package oscseminarday1;

class Box {
double width;
double height;
double depth;

// This is the constructor for Box.

Box() {
System.out.println("Constructing Box in constructor 1");
width = 10;
height = 10;
depth = 10;
}

Box(int w,int h,int d)  //overloaded constructor
    {
System.out.println("Constructing Box in constructor 2");
    width=w;
    height=h;
    depth=d;

}

// compute and return volume
double volume() {
return width * height * depth;
}
}

class ConstructorUsage{
public static void main(String args[]) {
// declare, allocate, and initialize Box objects
Box mybox1 = new Box();
Box mybox2 = new Box(10,20,30); //constructor overloading
System.out.println(mybox1.volume());
System.out.println(mybox2.volume());
}
}
