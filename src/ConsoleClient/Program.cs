using System;
using System.Collections.Generic;
using System.Linq;
using Netco.Logging;
using Netco.Logging.NLogIntegration;
using Cqrs.ConsoleClient.Actions;
using Cqrs.Infrastructure.Utils;

namespace Cqrs.ConsoleClient
{
	internal class Program
	{
		private static void Main( string[] args )
		{
			NetcoLogger.LoggerFactory = new NLogLoggerFactory();

			try
			{
				var actions = new ActionsFactory().CreateCommandsToExecute();
				PrintActionsToExecute( actions );
				ExecuteActionsWithConfirmation( actions );
			}
			catch( Exception exc )
			{
				NetcoLogger.GetLogger( typeof( Program ) ).Error( exc, "Global exception" );
			}
			Console.WriteLine( "Press ENTER to exit" );
			Console.ReadLine();
		}

		private static void ExecuteActionsWithConfirmation( IEnumerable< IAction > actions )
		{
			var input = GetInput( "Use [Azure] or [File(default)] storage? Or just E[x]it?" );
			switch( input )
			{
				case "x":
					return;
				case "Azure":
					if( actions.Any( a => a.WillUpdateData ) &&
						GetInput( "About to perform actions that will update data on PRODUCTION AZURE ENVIRONMENT!!!! CONTINUE!?!?!?! [YES!!]" ) != "YES!!" )
						return;
					EventStorageSettingsProvider.InitToLoadSettingsFromFile( "AzureStorage.config" );
					break;
				default:
					EventStorageSettingsProvider.InitToLoadSettingsFromFile( "FileStorage.config" );
					break;
			}

			foreach( var action in actions )
			{
				action.Execute();
			}
		}

		private static void PrintActionsToExecute( IEnumerable< IAction > actions )
		{
			Console.WriteLine( "Actions to perform" );
			foreach( var action in actions )
			{
				var consoleColor = Console.ForegroundColor;
				if( action.WillUpdateData )
					Console.ForegroundColor = ConsoleColor.Red;
				else
					Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine( action.ToString() );
				Console.ForegroundColor = consoleColor;
			}
		}

		private static string GetInput( string message = "" )
		{
			Console.Write( message + ">>>" );
			return Console.ReadLine();
		}
	}
}