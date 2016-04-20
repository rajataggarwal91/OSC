/* Program no 2
 * Program to show if and for statements
 * Also see how boolean is used
 * Also see how strings are concatenated and calculations are represented in println
 */


package oscseminarday1;

public class IfandFor {

    public static void main(String args[]) {
        boolean a = true;

        for (int i = 0; i < 10; i++) {
            if (a) {
                System.out.println((i + 1) + " hi"); //focus on usage of + as concatenator
            }
            //this will be incorrect 0 is not equivalent to false in java
            /*if (0) {
            System.out.println(i); */
        }
    }
}
