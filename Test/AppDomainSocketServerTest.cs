﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;
using SuperSocket.Common;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Provider;
using SuperSocket.SocketEngine;


namespace SuperSocket.Test
{
    [TestFixture]
    public class AppDomainSocketServerTest : TcpSocketServerTest
    {
        protected override IWorkItem CreateAppServer<T>(IRootConfig rootConfig, IServerConfig serverConfig)
        {
            var appServer = new AppDomainAppServer(typeof(T));

            var config = new ConfigurationSource();
            rootConfig.CopyPropertiesTo(config);

            appServer.Setup(new AppDomainBootstrap(config), serverConfig, new ProviderFactoryInfo[]
            {
                new ProviderFactoryInfo(ProviderKey.SocketServerFactory, ProviderKey.SocketServerFactory.Name, typeof(SocketServerFactory)),
                new ProviderFactoryInfo(ProviderKey.LogFactory, ProviderKey.LogFactory.Name, typeof(ConsoleLogFactory))
            });

            return appServer;
        }

        [Test]
        public void TestAppDomain()
        {
            StartServer();

            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), m_Config.Port);

            using (Socket socket = CreateClientSocket())
            {
                socket.Connect(serverAddress);
                Stream socketStream = GetSocketStream(socket);
                using (StreamReader reader = new StreamReader(socketStream, m_Encoding, true))
                using (ConsoleWriter writer = new ConsoleWriter(socketStream, m_Encoding, 1024 * 8))
                {
                    string welcomeString = reader.ReadLine();

                    Console.WriteLine("Welcome: " + welcomeString);

                    writer.WriteLine("DOMAIN");
                    writer.Flush();

                    var line = reader.ReadLine();

                    Assert.AreEqual(m_Config.Name, line);
                }
            }
        }
    }
}
