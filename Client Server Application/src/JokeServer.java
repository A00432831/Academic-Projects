/*--------------------------------------------------------
1. Pratik Gadhiya / 04/21/2019
2. Java version: build jdk-11.0.1
3. Precise command-line compilation examples / instructions:

> javac JokeServer.java
4.
In separate shell windows:
> java JokeServer
> java JokeClient
> java JokeClientAdmin
All acceptable commands are displayed on the various consoles.
5. How to use
For this server you only need to run it, and it will be listening to any potention clients
6. Files needed for running the program:
     JokeServer.java
     JokeClientAdmin.java
     JokeClient.java

7. Notes:


*/





import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;

// Server class
public class JokeServer {
    static boolean jokeMode=true;
    static boolean proverbMode;
    static int jokeCount;
    static int proverbCount = 0;
    static boolean isSecondary = false;
    static int port;

    public static void main(String a[]) throws IOException {
        int q_len = 6; /* Number of requested can be done at same time*/
        JokeServer.port = 4545;  // By default server works and create conncetion by port 4545.
        Socket sock;
    //    int secondaryPort = 4546;


        //this if condition awakes when there is request for
        // secondery server by keywork secondery and listens to 4546 port for client.
        if(a.length == 1)
            if(a[0].equals("secondary"))
            {
                JokeServer.port = 4546;
                isSecondary = true;
            }

        AdminLoop AL = new AdminLoop(); // DIFFERENT thread for Admin to be handled seperately.
        AL.isSecondary = isSecondary;
        Thread t = new Thread(AL);
        t.start();  // thread started
        ServerSocket servsock = new ServerSocket(JokeServer.port, q_len);

        System.out.println("Server Started.\n");
        while (true) {
            sock = servsock.accept();
            System.out.println("Server Listening to Pot number"+ JokeServer.port);
            new ClientHandler(sock).start();// a thread started for clients to handled seperately from admin.

           }
    }
}


// the thread comes ti this class and always listerns to for admin
// and also gtes conncts to secondery server by different port.
class AdminLoop implements Runnable {

    boolean isSecondary = false;

    public void run() { // listening to admin running
        DataOutputStream dos = null;
        DataInputStream dis = null;

        int q_len = 6; // Number of requested can be done at same time
        int port = 5050;  // We are listening at a different port for Admin clients other then client.
        Socket sock;
        try {
            ServerSocket servsock = new ServerSocket(port, q_len);
            sock = servsock.accept();
            new AdminHandler (sock).start();//adminhandler started for changing the modes.

            dis=new DataInputStream(sock.getInputStream());
            dos = new DataOutputStream(sock.getOutputStream());

            dos.writeUTF("Server one: Localhost, Port:5050");//printing the regular server port.
        } catch (IOException e) {
            e.printStackTrace();
        }
        try {                           //if secondary server started then it listern to admin via 5051 port.
            if(isSecondary)
                port = 5051;
            ServerSocket servsock = new ServerSocket(port, q_len);
            dos.writeUTF("Server two: Localhost, Port:5051");// printing the secondary server port.

            while (true) {
                // waiting for next admin to be connceted.
                sock = servsock.accept();
                new AdminHandler (sock).start();



            }
        } catch (IOException ioe) {
            System.out.println(ioe);
        }

    }
}

class AdminHandler extends Thread{    // using this class the  admin sets the mode of Server.
       Socket sock;

       int jokeCount;
       int proverbCount;

       AdminHandler(Socket s){
           sock=s;
       }

       public void run(){
           DataOutputStream dos=null;
           DataInputStream dis = null;
           //int portNumber=7777;
           try{


               dis=new DataInputStream(sock.getInputStream());
               dos = new DataOutputStream(sock.getOutputStream());
               dos.writeUTF(" AdminConnection Created:)");
               while(true) {                                       // as per the admin input the mode gets changed .
                   String recieve = dis.readUTF();
                   System.out.println(recieve);         //if admin press 1 it gets joke mode.
                   if (recieve.equals("1")) {
                       //System.out.println("Joke");
                       JokeServer.jokeMode = true;
                       JokeServer.proverbMode = false;
                   } else {                                 // if 2 is entered  mode gets to proverb mode.
                       //System.out.println("proverb");
                       JokeServer.proverbMode = true;
                       JokeServer.jokeMode = false;
                   }
               }
           }catch(Exception e)
           {System.out.println(e);}
       }
   }



    class ClientHandler extends Thread {          // this thread handles the client. and give response as per the mode.
        Socket client;
        String username;

        ClientHandler(Socket s) {
            client = s;
        }

        public void run() {
            DataOutputStream dos = null;
            DataInputStream dis = null;

            int modeCount;
            try {

                dis = new DataInputStream(client.getInputStream());
                dos = new DataOutputStream(client.getOutputStream());
                username = dis.readUTF();                           //username is fetched from client to server.
                System.out.println(username + " Connected");
                if (JokeServer.jokeMode==true) {                //it will check if jokemode then the client gets joke.
                    JokeServer.jokeCount++;                     //After counter gets incremented to send different joke to other clients.
                    String joke = getJoke();
                    if(JokeServer.port==4546){                  //this condition checks that on which port client is
                        dos.writeUTF("<S2>  "+joke);        //conncted and on that basis it writes <S2> in front of joke or proverb.
                    }
                    else{
                        dos.writeUTF(joke);
                    }

                }else if (JokeServer.proverbMode==true) {       //it will check if proverbmode then the client gets proverbs.
                    JokeServer.proverbCount++;                  //After counter gets incremented to send different proverb to other clients.
                    String proverb = getproverb();
                    if(JokeServer.port==4546){          //this condition checks that on which port client is conncted and on that basis it writes <S2> in front of joke or proverb.
                        dos.writeUTF("<S2>  "+proverb);
                    }
                    else{
                        dos.writeUTF(proverb);
                    }
                }

            } catch (Exception e) {
                System.out.println(e);
            }
        }

        public String getJoke() {                       // this method returns the joke as per the counter counts .
            String s = "Joke Mode";
            //jokeCount=1;
            try {
                if (JokeServer.jokeCount == 1) {
                    s = "JA " + username + ": Today at the bank, an old lady asked me to help check her balance. So I pushed her over. ";
                } else if (JokeServer.jokeCount == 2) {
                    s = "JB " + username + ": I'm so good at sleeping. I can do it with my eyes closed.\n";

                } else if (JokeServer.jokeCount == 3) {
                    s = "JC " + username + ": My boss told me to have a good day.. so I went home.";

                } else if (JokeServer.jokeCount == 4) {
                    s = "JD " + username + ": Did you hear about the italian chef that died? He pasta way.";

                    JokeServer.jokeCount = 0;

                }
            } catch (Exception e) {

            }
            return s;
        }

        public String getproverb() {                    // this method returns the proverbs as per the counter counts.
            String s = "Proverb mode";
            try {
                if (JokeServer.proverbCount == 1) {
                    s = "PA " + username + ": A bad workman always blames his tools.";

                } else if (JokeServer.proverbCount == 2) {
                    s = "PB " + username + ": A chain is only as strong as its weakest link.";

                } else if (JokeServer.proverbCount == 3) {
                    s = "PC " + username + ": Actions speak louder than words.";

                } else if (JokeServer.proverbCount == 4) {
                    s = "PD " + username + ": A journey of a thousand miles begins with a single step.";

                }
            } catch (Exception e) {

            }
            return s;
        }


    }

