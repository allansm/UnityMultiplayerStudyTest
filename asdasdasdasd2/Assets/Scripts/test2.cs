using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;
 
public class test2 : MonoBehaviour
{
	StreamWriter writer;
	StreamReader reader = null;
	public string identification = "none";
	public TcpClient sock;
	bool connected;
	public bool tryConnect;
	public bool tryDisconnect;
	public Animator animator;
	bool flag = true;
	float count = 0;
	IEnumerator cliRead;
	IEnumerator cliWrite;
	bool endCliRead = true;
	public string ip = "127.0.0.1";
	public string rec;
	public string pInfo;
	public bool updateIsRunning;
	public void connect(){
		try{
			sock = new TcpClient();
			sock.Connect(IPAddress.Parse(ip),9800);	
			writer = new StreamWriter(sock.GetStream());
			reader = new StreamReader(sock.GetStream());
			connected = true;
			print("connected");
		}catch(Exception e){
			connected = false;
		}
	}
	public void write(string txt){
		writer.WriteLine(txt);
		writer.Flush();
	}
	public string read(){
		return reader.ReadLine();
	}
	void inCli(){
		while(true){
			print("t working");
			if(connected){
				try{
					this.rec = read();
					print("read:"+rec);
				}catch(Exception e){
					print("error : "+e.Message);
				}
			}
			if(updateIsRunning){
				break;
			}
		}
	}
	void outCli(){
		while(true){
			print("t2 working");
			if(connected){
				try{
					if(pInfo != null){
						print("writed:"+pInfo);
						write(pInfo);
					}
				}catch(Exception e){
					connected = false;
					print("disconnect by error:"+e.Message);
				}
			}
			if(updateIsRunning){
				break;
			}
		}
	}
	IEnumerator inputThread(){
		//while(true){
			//print("running");
			//try{
				//print("try : "+connected);
				//if(connected){
					//print("running");
					rec = read();
					print("read:"+rec);
					string[] data = rec.Split(';');
					string id = data[0];
					if(id != identification){
						float x = float.Parse(data[1]);
						float y = float.Parse(data[2]);
						float z = float.Parse(data[3]);
						float rx = float.Parse(data[4]);
						float ry = float.Parse(data[5]);
						float rz = float.Parse(data[6]);
						float speed = float.Parse(data[7]);
						float direction = float.Parse(data[8]);
						bool jump = bool.Parse(data[9]);
						bool rest = bool.Parse(data[10]);
						float jumpHeight = float.Parse(data[11]);
						float gravityControl = float.Parse(data[12]);
					
						//print("id:"+id);
						//float f = 0.5f;
						//print("normal float:"+f);
						//print("x:"+x+" y"+y+" z"+z);
					
						GameObject temp;
						try{
							temp = GameObject.Find(id);
							temp.transform.position = new Vector3(x,y,z);
							temp.transform.rotation = Quaternion.Euler(rx,ry,rz);
							Animator tempAni = temp.GetComponent<Animator>();
							tempAni.SetFloat("Speed",speed);
							tempAni.SetFloat("Direction",direction);
							tempAni.SetBool("Jump",jump);
							tempAni.SetBool("Rest",rest);
							tempAni.SetFloat("JumpHeight",jumpHeight);
							tempAni.SetFloat("GravityControl",gravityControl);
						}catch(Exception e){
							temp = Instantiate(Resources.Load("unitychan"),new Vector3(x,y,z),Quaternion.Euler(rx,ry,rz)) as GameObject;
							temp.name = id;
							Animator tempAni = temp.GetComponent<Animator>();
							tempAni.SetFloat("Speed",speed);
							tempAni.SetFloat("Direction",direction);
							tempAni.SetBool("Jump",jump);
							tempAni.SetBool("Rest",rest);
							tempAni.SetFloat("JumpHeight",jumpHeight);
							tempAni.SetFloat("GravityControl",gravityControl);
						}
					}
				//}
			/*}catch(Exception e){
				print(e.Message);
				connected = false;
			}*/
			//endCliRead = true;
			yield return new WaitForSeconds(0);
		//}
	}
	IEnumerator outputThread(){
		try{
			string pInfo = 
			identification+";"+
			this.transform.position.x+";"+
			this.transform.position.y+";"+
			this.transform.position.z+";"+
			this.transform.eulerAngles.x+";"+
			this.transform.eulerAngles.y+";"+
			this.transform.eulerAngles.z+";"+
			animator.GetFloat("Speed")+";"+
			animator.GetFloat("Direction")+";"+
			animator.GetBool("Jump")+";"+
			animator.GetBool("Rest")+";"+
			animator.GetFloat("JumpHeight")+";"+
			animator.GetFloat("GravityControl");
			print("writed:"+pInfo);
			write(pInfo);
			
		}catch(Exception e){
			connected = false;
		}
		yield return new WaitForSeconds(0);
	}
	void setPlayersValues(){
		if(connected){
			if(rec != null){
				try{
					string[] data = rec.Split(';');
					string id = data[0];
					if(id != identification){
						float x = float.Parse(data[1]);
						float y = float.Parse(data[2]);
						float z = float.Parse(data[3]);
						float rx = float.Parse(data[4]);
						float ry = float.Parse(data[5]);
						float rz = float.Parse(data[6]);
						float speed = float.Parse(data[7]);
						float direction = float.Parse(data[8]);
						bool jump = bool.Parse(data[9]);
						bool rest = bool.Parse(data[10]);
						float jumpHeight = float.Parse(data[11]);
						float gravityControl = float.Parse(data[12]);
					
						GameObject temp;
						try{
							temp = GameObject.Find(id);
							temp.transform.position = Vector3.Slerp(temp.transform.position,new Vector3(x,y,z),/*Time.deltaTime*/0.05f*10);
							temp.transform.rotation = Quaternion.Euler(new Vector3(rx,ry,rz));
							Animator tempAni = temp.GetComponent<Animator>();
							tempAni.SetFloat("Speed",speed);
							tempAni.SetFloat("Direction",direction);
							tempAni.SetBool("Jump",jump);
							tempAni.SetBool("Rest",rest);
							//tempAni.SetFloat("JumpHeight",jumpHeight);
							//tempAni.SetFloat("GravityControl",gravityControl);
						}catch(Exception e){
							temp = Instantiate(Resources.Load("unitychan"),new Vector3(x,y,z),Quaternion.Euler(new Vector3(rx,ry,rz))) as GameObject;
							temp.name = id;
							Animator tempAni = temp.GetComponent<Animator>();
							tempAni.SetFloat("Speed",speed);
							tempAni.SetFloat("Direction",direction);
							tempAni.SetBool("Jump",jump);
							tempAni.SetBool("Rest",rest);
							//tempAni.SetFloat("JumpHeight",jumpHeight);
							//tempAni.SetFloat("GravityControl",gravityControl);
						}
					}
					rec = null;
				}catch(Exception e){
					
				}
			}
		}else{
			print("disconnected");
		}
	}
	void setOwnValues(){
		pInfo = 
		identification+";"+
		this.transform.position.x+";"+
		this.transform.position.y+";"+
		this.transform.position.z+";"+
		this.transform.eulerAngles.x+";"+
		this.transform.eulerAngles.y+";"+
		this.transform.eulerAngles.z+";"+
		animator.GetFloat("Speed")+";"+
		animator.GetFloat("Direction")+";"+
		animator.GetBool("Jump")+";"+
		animator.GetBool("Rest")+";"+
		animator.GetFloat("JumpHeight")+";"+
		animator.GetFloat("GravityControl");
	}
    // Start is called before the first frame update
    void Start()
    {
		
    }
	bool onlyOneTime = true;
	
    // Update is called once per frame
    void Update()
    {
		if(updateIsRunning && onlyOneTime){
			try{
				Thread t = new Thread(new ThreadStart(inCli));
				t.IsBackground = true;
				t.Start();
				Thread t2 = new Thread(new ThreadStart(outCli));
				t2.IsBackground = true;
				t2.Start();
			}catch(Exception e){
				print("wont start read and write");
			}
		}
		updateIsRunning = true;
		if(this.name != identification){
			this.name = identification;
		}
		setOwnValues();
		setPlayersValues();
		if(tryConnect){
			connect();
			tryConnect = false;
		}
		if(tryDisconnect){
			connected = false;
			tryDisconnect = false;
		}
    }
	bool showInt;
	void OnGUI(){
		if(showInt){
			GUI.Label(new Rect(10,10,200,20),"ip:");
			ip = GUI.TextField(new Rect(10,30,200,20),ip,25);
			GUI.Label(new Rect(10,50,200,20),"name:");
			identification = GUI.TextField(new Rect(10,70,200,20),identification,25);
			if(GUI.Button(new Rect(10,110,100,20),"Connect")){
				tryConnect = true;
				showInt = false;
			}
		}else{
			if(!connected){
				if(GUI.Button(new Rect(10,10,100,20),"Login")){
					showInt = true;
				}
			}else{
				if(GUI.Button(new Rect(10,10,100,20),"Disconnect")){
					tryDisconnect = true;
				}
			}
		}
	}
}
 
