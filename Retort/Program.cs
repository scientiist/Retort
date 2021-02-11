using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using NLua;

namespace Retort
{
	class Program
	{
		// retort.exe http://127.0.0.1:1234/test/ gameserverpinger.lua --verbose
		// retort.exe <listen_address> <handler_script> [args?]

		static string ManPage = @"retort - httprequest listener. usage: retort.exe <address> <script> [args]
args:
	--verbose	Toggles verbose output
";

		private static void RTFM() => Console.WriteLine(ManPage);

		static Lua LuaEnv;
		static LuaFunction Func;

		static void Main(string[] args)
		{
			LuaEnv = new Lua();
			LuaEnv.LoadCLRPackage();
			

			bool VerboseOutputEnabled = false;

			if (args.Length < 2)
			{
				RTFM();
				return;
			}

			var address = args[0];
			var script = args[1];


			var firstReturnedLuaValue = LuaEnv.DoFile(script)[0] as LuaFunction;


			Func = firstReturnedLuaValue;

			for (int argid = 2; argid < args.Length; argid++) 
			{
				var argument = args[argid];
				if (argument == "--verbose")
					VerboseOutputEnabled = true;
			}


			var httpListener = new HttpListener();
			var serv = new RetortServer(httpListener, address, Response);

			serv.VerboseOutput = VerboseOutputEnabled;

			serv.Start();



			using (var wb = new WebClient())
			{
				var data = new NameValueCollection();
				data["username"] = "myUser";
				data["password"] = "myPassword";

				var response = wb.UploadValues(address, "POST", data);
				string responseInString = Encoding.UTF8.GetString(response);
				Console.WriteLine("R:" + responseInString);
			}


		}
		public static byte[] Response(string request)
		{
			Console.WriteLine(request);
			var retVal = Func.Call(request);
			return Encoding.ASCII.GetBytes(retVal[0] as string);
		}
	}
}
