/* Program no.3
 * Program shows how to take a string and other data type inputs from console
 */


package oscseminarday1;

import java.io.*;       //note the usage of import to include classes under io package

public class InputProgram {

    public static void main(String args[]) throws IOException {
        String st;
        int num;
        System.out.println("Write something and I will echo it:  ");
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
        st = br.readLine();
        System.out.println(st);
        System.out.println("Now enter an integer:");
        num=Integer.parseInt(br.readLine());
        System.out.println("Double="+(2*num));

    }
}
