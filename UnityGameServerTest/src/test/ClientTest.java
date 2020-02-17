package test;

import java.io.PrintWriter;
import java.net.Socket;
import java.util.Scanner;

public class ClientTest {
	public static void main(String[]args) {
		try {
			System.out.println("try port 9800");
			Socket socket = new Socket("127.0.0.1",9800);
			Scanner scan = new Scanner(socket.getInputStream());
			System.out.println("connected");
			new Thread(new Runnable() {
				
				@Override
				public void run() {
					// TODO Auto-generated method stub
					while(true) {
						if(scan.hasNext()) {
							System.out.println("server:"+scan.nextLine());
						}
					}
				}
			}).start();
			String name = null;
			PrintWriter writer = new PrintWriter(socket.getOutputStream());
			while(true) {
				System.out.println("writer assembled");
				Scanner s = new Scanner(System.in);
				if(name == null) {
					System.out.print("your name:");
					name = s.nextLine();
				}
				System.out.print("txt:");
				String txt = s.nextLine();
				writer.print(name+":"+txt+"\n");
				writer.flush();
			}
		}catch(Exception e) {
			e.printStackTrace();
		}
	}
}
