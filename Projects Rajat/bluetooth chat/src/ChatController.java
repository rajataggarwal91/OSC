import javax.microedition.midlet.*;
import javax.microedition.lcdui.*;
import javax.bluetooth.*;
import java.io.IOException;
import javax.microedition.io.Connector;

public class ChatController extends MIDlet implements CommandListener
{
 private ChoiceGroup devices=null;
 private Display display = null;
 private Form mainForm = null;
 private StringItem inTxt = null;
 private TextField outTxt = null;
 private Command exit = null;
 private Command connect = null;
 private Command send = null;
 private Command select = null;
 private Command once=null;
 private StringItem status = null;
 private Command ok=null;
 private Command ok2=null;
 private TextBox nameBox=null;
 private TextField DNo=null;
 private LocalDevice local = null;
 private RemoteDevice rDevices[];
 private ServiceRecord service = null;
 private DiscoveryAgent agent = null;
 private L2CAPConnectionNotifier notifier;
 private L2CAPConnection connection = null;
 private String DevNo= new String();
 private static final String UUID_STRING = "112233445566778899AABBCCDDEEFF";
 private String Dname=new String();
 private boolean running = false;
private int chance=0;
private int index=0;
 public ChatController()
	{
     super();
     display = Display.getDisplay(this);

	 mainForm = new Form("CHAT");
         devices = new ChoiceGroup(null,Choice.EXCLUSIVE);
         inTxt = new StringItem("incoming msg:","");
	 outTxt = new TextField("outgoing msg:","",256,0);
	 exit = new Command("EXIT",Command.EXIT,1);
	 connect = new Command("CONNECT",Command.SCREEN,2);
	 send = new Command("SEND",Command.OK,1);
	 select = new Command("SELECT",Command.SCREEN,2);
	 ok=new Command("OK",Command.OK,1);
         status = new StringItem("status : ","");
	 mainForm.append(status);
	 mainForm.addCommand(exit);
	 mainForm.setCommandListener(this);
         nameBox=new TextBox("User Name:","",32,0);
         DNo=new TextField("Device no.","",5,TextField.NUMERIC);
         ok2=new Command("OK",Command.OK,1);
         once = new Command("once",Command.OK,1);
 }


 protected void startApp() throws MIDletStateChangeException
	{
     Ticker t=new Ticker("Projectees:1.Rahul 2.Binay 3.Rajat 4.Chetan Guided by:Prof. R. Damdoo");
     running = true;
     nameBox.addCommand(ok);
     display.setCurrent(nameBox);
     nameBox.setTicker(t);
     nameBox.addCommand(ok);
     nameBox.setCommandListener(this);

     try
       {
        local = LocalDevice.getLocalDevice();
	agent = local.getDiscoveryAgent();
       }
     catch(BluetoothStateException bse)
       {
        status.setText("BluetoothStateException unable to start:"+bse.getMessage());
		try
		  {
		   Thread.sleep(1000);
                  }
	 	catch(InterruptedException ie)
		  {}
        notifyDestroyed();
       }
    }

 protected void pauseApp()
	{
     running = false;
     releaseResources();
	}

 protected void destroyApp(boolean uncond) throws MIDletStateChangeException
	{
     running = false;
     releaseResources();
    }

 public void commandAction(Command cmd,Displayable disp)
	{
     if(cmd==exit)
		{
	     running = false;
             releaseResources();
	     notifyDestroyed();
       	        }
      else if(cmd==connect)
          {
                 if(DevNo.equals("1")||DevNo.equals("3"))
                     commandAction(once,mainForm);
                 mainForm.removeCommand(ok);
                 mainForm.addCommand(select);
              if(DevNo.equals("0")||DevNo.equals("2"))
                {
                 new Thread()
                   {
                     public void run()
                     {
                         startServer();
                     }
                 }.start();
                  return;
              }
              status.setText("searching for devices...");
              mainForm.removeCommand(connect);
              DeviceDiscoverer discoverer = new DeviceDiscoverer(ChatController.this);
				  try
				     {
				  	  agent.startInquiry(DiscoveryAgent.GIAC,discoverer);
                                          System.err.println("Searching for devices block");
                                  }

				  catch(Exception ed)
				     {
                                  status.setText("BluetoothStateException-- :"+ed.getMessage());
				  System.err.println("error in device discovery block");
                                  }
			//commandAction(select,mainForm);


      }

      else if (cmd==once)
      {
          new Thread()
                {
                    public void run()
                    {

                        try {
                        while(running)
                        {
                            Thread.sleep(5000);
                            commandAction(select,mainForm);

                        }
                        } catch (InterruptedException ex) {
                            ex.printStackTrace();
                        }
                    }
                }.start();
      }
          else if (cmd==select)
          {
                System.err.println("In select Command block!!!");
                 
                status.setText("searching devices for service...");
                 if (DevNo.equals("1") && chance == 0) {
                    chance = 1;
                    index = 0;
                } else if (DevNo.equals("1") && chance == 1) {
                    chance = 0;
                    index = 1;
                } else if (DevNo.equals("3") && chance == 0) {
                    chance = 1;
                    index = 1;
                } else if (DevNo.equals("3") && chance == 1) {
                    chance = 0;
                    index = 0;
                }
                ServiceDiscoverer serviceDListener = new ServiceDiscoverer(ChatController.this);
                int[] attrSet = {0x0100}; //returns service name attribute
                UUID[] uuidSet = {new UUID(UUID_STRING, false)};
                try {
                    System.err.println("In Service discovery block");
                    
                    agent.searchServices(attrSet, uuidSet, rDevices[index], serviceDListener);
                    }
                catch (Exception npe1)
                {
                    System.err.println("Problem in service discovery block"+npe1);
                    status.setText(npe1.getMessage());
                }
         }
      
          else if(cmd==send)
            {
	       new Thread()
                 {
		    public void run()
                     {
		     sendMessage();
		     }
		  }.start();
	 }
                      else if(cmd==ok)
                      {
                          display.setCurrent(mainForm);
                          Dname=nameBox.getString();
                          mainForm.deleteAll();
                          mainForm.append(DNo);
                          display.setCurrent(mainForm);
                          mainForm.addCommand(ok2);
                       }

                      else if (cmd==ok2)
                      {

                          DevNo=DNo.getString();
                          mainForm.deleteAll();
                          mainForm.append(status);
                          commandAction(connect,mainForm);
                          mainForm.setCommandListener(this);
                          mainForm.removeCommand(ok2);
                      }

	 }

//this method is called from DeviceDiscoverer when device inquiry finishes
 public void deviceInquiryFinished(RemoteDevice[] rDevices,String message)
	{
     System.err.println("deviceeEnquiryFinished");
     this.rDevices = rDevices;
	 String deviceNames[] = new String[rDevices.length];
	 for(int k=0;k<rDevices.length;k++)
		{
	     try
	       {
	     	deviceNames[k] = rDevices[k].getFriendlyName(false);
	       }
	     catch(IOException ioe)
	       {
	        status.setText("IOException1 :"+ioe.getMessage());
		   }
	    }
         for(int l=0;l<deviceNames.length;l++)
		{
	     devices.append(deviceNames[l],null);
	    }
      status.setText(message);
    }

//called by ServiceDiscoverer when service search gets completed
 public void serviceSearchFinished(ServiceRecord service,String message)
	{
     System.err.println(" in Servicesearchfinished!!!");
     String url = "";
     this.service = service;
	 status.setText(message);
	 try
	   {
	    url = service.getConnectionURL(ServiceRecord.NOAUTHENTICATE_NOENCRYPT,false);
            System.err.println("url assifgnment executed");
	   }
	 catch (IllegalArgumentException iae1)
	   {System.err.println("Exception at url!!");}
     try
       {
     	connection = (L2CAPConnection)Connector.open(url);
		status.setText("connected to device "+index);
		sendAllMessage();

                new Thread()
		   {
		    public void run()
			   {
			    startReceiver();
                            mainForm.removeCommand(select);
                            
                    }
		   }.start();
       }
     catch(IOException ioe1)
       {
        status.setText("IOException2 :"+ioe1.getMessage());
        System.err.println("Error in connection open at client and starting receiver");
     }
	}

 // this method starts L2CAPConnection chat server from server mode
 public void startServer()
	{
     status.setText("server starting...");
	 mainForm.removeCommand(connect);
         mainForm.removeCommand(select);
        try
	   {
	 	local.setDiscoverable(DiscoveryAgent.GIAC);

		notifier = (L2CAPConnectionNotifier)Connector.open("btl2cap://localhost:"+UUID_STRING+";name=L2CAPChat");

		ServiceRecord record = local.getRecord(notifier);
		String conURL = record.getConnectionURL(ServiceRecord.NOAUTHENTICATE_NOENCRYPT,false);
		status.setText("server running...");

                new Thread()
                {
                    public void run()
                    {
                while(running)
                    {
                        try {
                            connection = notifier.acceptAndOpen();
                            inTxt.setText("");
                            System.err.println("Starting receiver now!!!");
                            startReceiver();

                        } catch (IOException ex) {
                            ex.printStackTrace();
                        }

                    }
                  }

                }.start();
            
        }
	 catch(IOException ioe3)
	   {
        status.setText("IOException3 :"+ioe3.getMessage());
	   }
 


 }

 //starts a message receiver listening for incomming message
 public void startReceiver()
	{
     System.err.println("startReceiver block!!");
     display.setCurrent(mainForm);
     mainForm.deleteAll();
     mainForm.append(status);
     mainForm.addCommand(send);
     mainForm.append(inTxt);
     mainForm.append(outTxt);
     mainForm.removeCommand(select);
       
     try
	        {
	     	 
                                StringBuffer inbuf= new StringBuffer("");
	                        String inmsg=new String("");
                                int receiveMTU = connection.getReceiveMTU();
				byte[] data = new byte[receiveMTU];
                                int length = connection.receive(data);
                      		String message = new String(data,0,length);
                                 System.err.println("This is mssage: "+message);
                                inbuf.append(inTxt.getText());
                                inbuf.append(message);
                                inmsg+=inbuf;
                                inTxt.setText(inmsg);
		
             }
             catch(IOException ioe4)
	        {
	        status.setText("IOException4 :"+ioe4.getMessage());
                }
	 
     
 }

 //sends a message over L2CAP
 public void sendMessage()
	{
     StringBuffer inbuf= new StringBuffer("");
     try
       {
                String message = Dname + ":" + outTxt.getString()+"\n";
	 	String inmsg=new String("");
                inbuf.append(inTxt.getText());
                inbuf.append(message);
                inmsg+=inbuf;
                inTxt.setText(inmsg);
                byte[] data = message.getBytes();
		int transmitMTU = connection.getTransmitMTU();
		if(data.length <= transmitMTU)
		 {
		  connection.send(data);
		  outTxt.setString("");
                }
        else
		 {
		  status.setText("message ....");
		 }
       }
     catch (IOException ioe5)
       {
        status.setText("IOException5 :"+ioe5.getMessage());
       }
    }

 //closes L2CAP connection
 public void releaseResources()
	{
     try
       {
     	if(connection != null)
        	connection.close();
        if(notifier!=null)
                notifier.close();
     }
     catch(IOException ioe6)
       {
        status.setText("IOException6 :"+ioe6.getMessage());

      }
   
 }

 public void sendAllMessage()
 {
     StringBuffer inbuf= new StringBuffer("");
     try
       {
	 	String inmsg=new String("");
                inbuf.append(inTxt.getText());
                inmsg+=inbuf;
                inTxt.setText(inmsg);
                byte[] data = inTxt.getText().getBytes();
		int transmitMTU = connection.getTransmitMTU();
		if(data.length <= transmitMTU)
		 {
		  connection.send(data);
		  }
        else
		 {
		  status.setText("message ....");
		 }
       }
     catch (IOException ioe5)
       {
        status.setText("IOException5 :"+ioe5.getMessage());
       }

 }




 }




