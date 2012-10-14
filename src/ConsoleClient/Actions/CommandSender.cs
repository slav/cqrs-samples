using Abstractions;
using Netco.Logging;
using Cqrs.Infrastructure.Client;
using Cqrs.Infrastructure.Messaging;

namespace Cqrs.ConsoleClient.Actions
{
	public class CommandSender< TCommand > : IAction where TCommand : ICommand
	{
		public TCommand Command{ get; private set; }
		public IMessageSender< TCommand > Destination { get; private set; }
		public bool IdFromContent{ get; private set; }
		public bool WillUpdateData{ get{ return true; }}

		public CommandSender( TCommand command )
		{
			this.Command = command;
		}

		public CommandSender< TCommand > To( IClient< TCommand > clientDestination )
		{
			this.Destination = clientDestination.Send;
			return this;
		}

		public CommandSender< TCommand > WithIdDerivedFromContent()
		{
			this.IdFromContent = true;
			return this;
		}

		public void Execute()
		{
			this.Log().Info( this.ToString() );
			this.IdFromContent = true;
			this.Destination.SendCommand( this.Command, this.IdFromContent );
		}

		public override string ToString()
		{
			return "Send: " + this.Command;
		}
	}
}