package test;

import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class ServerTest {

	public ServerSocket server;
	public List<Scanner> scans;
	public List<PrintWriter> prints;
	public int connectedCount = 0;
	public List<String> txt;
	private String msg;

	public boolean sendedToAll;
	
	public synchronized String getMsg() {
		return this.msg;
	}
	public synchronized void setMsg(String msg) {
		this.msg = msg;
	}
	public void inputServer() {
		
		try {
			server = new ServerSocket(9800);
			scans = new ArrayList<Scanner>();
			txt = new ArrayList<String>();
			prints = new ArrayList<PrintWriter>();
		}catch(Exception e) {
			System.err.println("error creating server :"+e.getMessage());
		}
		
		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				while(true) {
					try {
						Thread.sleep(5);
						Socket sock = server.accept();
						Scanner s = new Scanner(sock.getInputStream());
						PrintWriter w = new PrintWriter(sock.getOutputStream());
						scans.add(s);
						prints.add(w);
						new Thread(new Runnable() {
							@Override
							public void run() {
								// TODO Auto-generated method stub
								Scanner cli = s;
								while(true) {
									if(cli.hasNext()) {
										String text = cli.nextLine();
										setMsg(text);
										//txt.add(text);
										//System.out.println(getMsg());
									}
								}
							}
						}).start();
						System.out.println("total client:"+(++connectedCount));
					}catch(Exception e) {
						System.err.println("error add scan :"+e.getMessage());
					}
				}
			}
		}).start();
		
	}
	public void outputServer() {
		new Thread(new Runnable() {
			
			@Override
			public void run() {
				while(true) {
					//System.out.println("outputWorking.. msg:"+msg);
					try {
						Thread.sleep(5);
						String msgCp = getMsg();
						//if(msgCp != null) {
							//System.out.println("start sending..");
							for(PrintWriter w:prints) {
								Thread.sleep(1);
								new Thread(new Runnable() {
									@Override
									public void run() {
										// TODO Auto-generated method stub
										//if(getMsg() != null && !getMsg().equals(getLastMsg())) {
											System.out.println(msgCp);
											sendedToAll = true;
											w.println(msgCp);
											w.flush();
										//}
									}
								}).start();
							//}
							//setMsg(null);
						}
					}catch(Exception e) {
						System.err.println("error:"+e.getMessage());
					}
				}
				
			}
		}).start();
	}
	public static void main(String[]args) {
		ServerTest server = new ServerTest();
		server.inputServer();
		server.outputServer();
	}
}
