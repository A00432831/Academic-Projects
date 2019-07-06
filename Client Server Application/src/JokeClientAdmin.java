/*--------------------------------------------------------
1. Gadhiya Pratik
2. Java version: build 1.8.0
3. Instructions:

> java JoketClientAdmin
then (assuming the server is running)
You will be prompted to enter a Mode, after which the Admin Client will request the server to change modes.
Example(s):
JokeMode
ProverbMode
MaintenanceMode
Also by typing in Quit it will exit the Admin Client
4.
In separate shell windows:
> java JokeServer
> java JokeClient
> java JokeClientAdmin
All acceptable commands are displayed on the various consoles.
5. Files needed for running the program:
        JokeServer.java
        JokeClientAdmin.java
        JokeClient.java

5. Notes:

N/A
----------------------------------------------------------*/


import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.InetAddress;
import java.net.Socket;
import java.util.Scanner;

public class JokeClientAdmin
{
    public static void main(String[] args) throws IOException
    {
        try
        {
            Scanner scn = new Scanner(System.in);
            InetAddress ip = InetAddress.getByName("localhost");//getting the ip of the localhost
            Socket s = new Socket(ip, 5050);            // in socket by port and ip admin gets conncted to server.

            // getting output and sending input by streams
            DataInputStream dis = new DataInputStream(s.getInputStream());
            DataOutputStream dos = new DataOutputStream(s.getOutputStream());

            // the following loop performs the exchange of
            // information between admin and server regarding changing the mode of server.
            String rec = dis.readUTF();
            System.out.println(rec);
            while(true)
            {
                //rec = dis.readUTF();
                //System.out.println(rec);
                //System.out.println("Loop running");
                String tosend;
                boolean inputValid =true;
                do {
                    System.out.println("Select the Mode you want to be on Server:");
                    System.out.println("Enter 1 for joke mode and 2 for Proverb Mode " +
                            "and Exit to close the the connection");
                    tosend = scn.nextLine();
                    if (tosend.equals("1") || tosend.equals("2")) {         //on basis of your input the mode of the server gets changed.
                        //System.out.println("Valid Input");
                        if(tosend.equals("1")){
                            System.out.println("Joke Mode Selected");
                        }
                        else
                            System.out.println("Proverb Mode Selected");
                     dos.writeUTF(tosend);
                    } else {
                        //System.out.println("Invalid input");
                    }
                }while(!(tosend.equals("Exit")));               //by typing Exit the connction gets terminated.
                s.close();
                System.out.println("Exited by Admin");
                break;
            }

            // closing resources
            scn.close();
            dis.close();
            dos.close();
            s.close();
        }catch(Exception e){
            e.printStackTrace();
        }
    }
}


