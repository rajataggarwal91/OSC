package oscseminarday1;

import java.io.*;

public class Stopwatch {

    public static void main(String args[]) throws IOException, InterruptedException {
        int time;
        System.out.println("Enter time in sec:");
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
        time = Integer.parseInt(br.readLine());
        for (int i = time; i > 0; i--) {
            System.out.println("Time left:" + i);
            Thread.sleep(1000);
        }
        System.out.println("Time is over!!!");
    }
}
