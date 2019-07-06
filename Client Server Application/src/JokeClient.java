/*--------------------------------------------------------
1. Pratik Gadhiya/ 04/21/2015
2. Java version: build jdk-11.0.1
3. Precise command-line compilation examples / instructions:

> javac JokeClient.java
4.
In separate shell windows:
> java JokeServer
> java JokeClient
> java JokeClientAdmin
All acceptable commands are displayed on the various consoles.
5. How to use
For this client, assuming the server is running,
    Enter your user name.
    Then client will automatically gets the JOke or Proverb depending on the Mode of Server.
    You you want to Disconnect from Server type 'Exit'

6. Files needed for running the program:
        JokeServer.java
        JokeClientAdmin.java
        JokeClient.java

7. Notes:

N/A
----------------------------------------------------------*/


import java.io.*;
import java.net.*;
import java.util.Scanner;

// Client class
public class JokeClient
{
    public static void main(String[] args) throws IOException
    {
        try
        {
            Scanner scn = new Scanner(System.in);
            InetAddress ip;
            Socket s;

            try {
                ip = InetAddress.getByName("localhost"); //getting the ip of the localhost


                s = new Socket(ip, 4545);           // if the server listerns on port 4545 it gets connceted.
            }
            catch(Exception e){
                ip= InetAddress.getByName("localhost");
                s = new Socket(ip, 4546);           //if server listerns on port 4546 then client gets conncted by port 4546.
                //System.out.println("Entered in catch block");
               // e.printStackTrace();
            }
            // getting output and sending input by streams
            DataInputStream dis = new DataInputStream(s.getInputStream());
            DataOutputStream dos = new DataOutputStream(s.getOutputStream());

            // the following loop performs the exchange of
            // information between client and server
            System.out.println("Enter your Username:");
            String username = scn.nextLine();
            dos.writeUTF(username);
            while (true)
            {
                System.out.println(dis.readUTF());
                System.out.println("Enter Exit to close the Connection.");
                String tosend = scn.nextLine();
                dos.writeUTF(tosend);

                // If client sends exit,close this connection
                // and then break from the while loop

                if(tosend.equals("Exit"))
                {
                    System.out.println("Closing this connection : " + s);
                    s.close();
                    System.out.println("Connection closed");
                    break;
                }

                // printing joke or proverb as per server mode.
                String received = dis.readUTF();
                System.out.println(received);
            }

            // closing resources
            scn.close();
            dis.close();
            dos.close();
        }catch(Exception e){
            //InetAddress ip = InetAddress.getByName("localhost");
            //Socket s = new Socket(ip, 4546);
            //System.out.println("Entered in catch block");
            e.printStackTrace();
        }
    }
}