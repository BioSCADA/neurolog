/*M///////////////////////////////////////////////////////////////////////////////////////
//
//  IMPORTANT: READ BEFORE DOWNLOADING, COPYING, INSTALLING OR USING.
//
//  By downloading, copying, installing or using the software you agree to this license.
//  If you do not agree to this license, do not download, install,
//  copy or use the software.
//
//
//                           License Agreement
//                For Open Source BioSCADA® Library  
//
// Copyright (C) 2011-2012, Diego Schmaedech, all rights reserved. 
//
							For Open Source SCADA for Human Data
//
// Copyright (C) 2012, Laboratório de Educação Cerebral, all rights reserved.
//
// Copyright (C) 2013, CogniSense Tecnologia Ltda, all rights reserved.
// Third party copyrights are property of their respective owners.
// Third party copyrights are property of their respective owners.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
//   * Redistribution's of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//
//   * Redistribution's in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//
//   * The name of the copyright holders may not be used to endorse or promote products
//     derived from this software without specific prior written permission.
//
// This software is provided by the copyright holders and contributors "as is" and
// any express or implied warranties, including, but not limited to, the implied
// warranties of merchantability and fitness for a particular purpose are disclaimed.
// In no event shall the Intel Corporation or contributors be liable for any direct,
// indirect, incidental, special, exemplary, or consequential damages
// (including, but not limited to, procurement of substitute goods or services;
// loss of use, data, or profits; or business interruption) however caused
// and on any theory of liability, whether in contract, strict liability,
// or tort (including negligence or otherwise) arising in any way out of
// the use of this software, even if advised of the possibility of such damage.
//
//M*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using Neurolog.Blueteeth;
using BioSCADA;
namespace Neurolog
{
    public class TCPDriver
    {

        const int MAX_CLIENTS = 10;
        int port = 9999;
       
        public Boolean IsConnected = false;
        Socket m_mainSocket;
        public String strClients = "";
        private List<StateObject> m_workerSocket = new List<StateObject>();

        public class StateObject
        {
            public Socket socket = null;	// Client socket.
            public const int BufferSize = 1024;	// Size of receive buffer.
            public byte[] buffer = new byte[BufferSize];// Receive buffer.
            public StringBuilder sb = new StringBuilder();//Received data String.
            public int id = -1; // client id.
        }

        public TCPDriver()
        {
             
        }
          
        public Boolean startListening(int port)
        { 
            try
            {
                this.port = port;
                m_mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port); 
                m_mainSocket.Bind(ipLocal); 
                m_mainSocket.Listen(1000); 
                m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                IsConnected = true;

                Console.WriteLine("KoanGear is listening...");
            }
            catch (SocketException se)
            {
                IsConnected = false;
                Console.WriteLine(se.Message);
            }
            return IsConnected;
        }

        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            { 
                Socket handler = m_mainSocket.EndAccept(asyn);
                StateObject state = new StateObject();
                state.socket = handler;
                state.id = m_workerSocket.Count;
                m_workerSocket.Add(state);
                try
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
                catch (SocketException se)
                {
                    MessageBox.Show(se.Message);
                }
                strClients += String.Format("Cliente # {0} conectado! ", m_workerSocket.Count - 1);
                strClients += System.Environment.NewLine;
                Console.WriteLine(strClients);

                m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                sendMessage("Welcome to KoanGear Connector!\n");

            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine( "OnClientConnection: Socket has been closed" ); 
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }

        protected void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty; 
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.socket;
            int id = state.id;
            try
            { 
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
                    content = state.sb.ToString();
                    if ((content.Length > 0) || (content.IndexOf("") > -1))
                    {
                        String strh = String.Format("Koan Client # {0} data: ", id);
                        String strRR = content.Replace("\n\0", "");
                        strh += content.Replace("\n\0", "");
                        Console.WriteLine(strRR);
                        if (strRR.StartsWith("PROTOCOL"))
                        {                             
                            sendMessage("RR|CO|RAN\n");
                        }
                        if (strRR.StartsWith("RR"))
                        {
                            String rr = Protocol.Meditation.ToString() ;
                            sendMessage(rr+ "\n"); 
                        }
                        if (strRR.StartsWith("CO"))
                        {
                            String co = Protocol.Coherence.ToString();
                            sendMessage(co + "\n");
                        }
                        if (strRR.StartsWith("RAN")) {

                            float f = 50;
                            float a = 200;


                            double y = 900 + a * Math.Sin(Protocol.Meditationlist.Count * ((2 * Math.PI) / f));  
                  
                           sendMessage( Convert.ToInt32(y) + "\n");
                           Console.WriteLine(Convert.ToInt32(y) + "\n");
                        }
                        Console.WriteLine(strh);
                        state.sb.Length = 0;



                        //Send the incoming string to all current connections (remove if you don't want);
                        //////////////////////////////////////////////////////////////////////
                        Object objData = content.Replace("\0", "");
                        byte[] byData = System.Text.Encoding.UTF8.GetBytes(objData.ToString() + "\0");
                        for (int i = 0; i < m_workerSocket.Count; i++)
                        {
                            if ((m_workerSocket[i] != null) && (m_workerSocket[i].socket.Connected) && (m_workerSocket[i].id != state.id))
                            {
                                m_workerSocket[i].socket.Send(byData);
                            }
                        }
                        /////////////////////////////////////////////////////////////////


                    }
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(this.ReadCallback), state);

                }
                else
                {
                    closeSocket(state.id);
                }

            }
            catch (System.Net.Sockets.SocketException es)
            {
                closeSocket(state.id);
                if (es.ErrorCode != 64)
                {
                    Console.WriteLine( string.Format("Socket Exception: {0}, {1}.", es.ErrorCode, es.ToString()) );
                }
            }
            catch (Exception e)
            {
                closeSocket(state.id);
                if (e.GetType().FullName != "System.ObjectDisposedException")
                {
                   Console.WriteLine( "Exception: {0}.", e.ToString() );
                }
            }
        }

        private void closeSocket(int id)
        {
            strClients += String.Format("Cliente # {0} desconectado!", m_workerSocket[id].id);
            strClients += System.Environment.NewLine;
            m_workerSocket[id].socket.Close();
            m_workerSocket[id] = null;
            Console.WriteLine(strClients);
        }

        public String getHostName()
        {
            String strHostName = Dns.GetHostName(); 
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName); 
            String IPStr = "";
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                IPStr = ipaddress.ToString();
                return strHostName;
            }
            return strHostName;
        }

        private void closeSockets()
        {

            if (m_mainSocket != null)
            {
                m_mainSocket.Close();
            }
            for (int i = 0; i < m_workerSocket.Count; i++)
            {
                if (m_workerSocket[i] != null)
                {
                    m_workerSocket[i].socket.Close();
                    m_workerSocket[i] = null;
                }
            }
            IsConnected = false;
            MessageBox.Show("A comunicação foi encerrada!", "Info!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            strClients = "";
        }

        private void sendMessage(string msg)
        {
            try
            {

                byte[] byData = System.Text.Encoding.UTF8.GetBytes(msg + "\0");
                for (int i = 0; i < m_workerSocket.Count; i++)
                {
                    if ((m_workerSocket[i] != null) && (m_workerSocket[i].socket.Connected))
                    {
                        m_workerSocket[i].socket.Send(byData);
                    }
                }

            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }
         
        public void closeDriver()
        {
            closeSockets();
        }


    }
}
