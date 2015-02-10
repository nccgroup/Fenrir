# Fenrir
Fenrir was created to prove that it is possible to tunnel network traffic over an RDP virtual channel, however it is still in its infancy. This PoC was to designed to tunnel the meterpreter reverse http shell over the virtual channel The long term goal here will be to create a fully-fledged socks proxy, from Loki to Fenrir and vice versa.

## Usage
Fenrir is very simple to use, use the “Metasploit Server” and “Metasploit Port” fields on Loki’s initial interface to specify the location of the users metasploit listener, and enter the port number you want to listen on the RDP server (default is 3000).
 
Once the “Start Forwarder” button is clicked Fenrir checks if the port is available and if so opens a listening port on 127.0.0.1
Any traffic sent to this port is forwarded on to the server and port that the user specified in the initial Loki interface.
