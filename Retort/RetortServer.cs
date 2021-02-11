using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Retort
{

	public delegate byte[] ProcessDataDelegate(string data);
	public class RetortServer
	{

		public bool VerboseOutput { get; set; }


		private const int HandlerThread = 2;
		private readonly ProcessDataDelegate handler;
		private readonly HttpListener listener;
		public RetortServer(HttpListener listener, string url, ProcessDataDelegate handler)
		{
			this.listener = listener;
			this.handler = handler;
			listener.Prefixes.Add(url);
		}

		public void Start()
		{
			if (listener.IsListening)
				return;


			listener.Start();

			for (int i = 0; i < HandlerThread; i++)
			{
				listener.GetContextAsync().ContinueWith(ProcessRequestHandler);
			}
		}

		public void Stop()
		{
			if (listener.IsListening)
				listener.Stop();
		}

		private void ProcessRequestHandler(Task<HttpListenerContext> result)
		{
			var context = result.Result;

			if (!listener.IsListening)
				return;

			listener.GetContextAsync().ContinueWith(ProcessRequestHandler);
			string request = new StreamReader(context.Request.InputStream).ReadToEnd();


			var responseBytes = handler.Invoke(request);
			context.Response.ContentLength64 = responseBytes.Length;


			var output = context.Response.OutputStream;
			output.WriteAsync(responseBytes, 0, responseBytes.Length);
			output.Close();
		}
	}
}
