/* Program no 4
 * Program to show use of new operator and construction of arrays
 * arrays in Java are implemented as objects
 */
package oscseminarday1;

public class Usingnew {

    public static void main(String args[]) {
        int a;      //primitive int
        int b[] = new int[3]; //declares an array object of size 3 dynamically
        int c[][] = new int[3][3]; //multidimensional array

        for (int i = 0; i < 3; i++) {
            b[i] = i;
            System.out.println(b[i]);
        }
        //assigning multidimensional elements their values
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                c[i][j] = i + j;

            }
        }
        //printing multidimensional array
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                System.out.print(c[i][j] + " ");
            }
            System.out.println("");     //for new line
        }


    }
}
