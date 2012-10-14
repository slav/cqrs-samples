using Channels.Contracts;
using Data.Contracts;
using Foundation.Contracts;
using Inventory.Contracts;
using Lokad.Cqrs;
using Receiving.Contracts;
using Cqrs.Infrastructure.Client;
using Wires.Config;

namespace Cqrs.ConsoleClient
{
	public static class Global
	{
		public static readonly IClient< IFoundationCommand > Foundation;
		public static readonly IClient< IChannelsCommand > Channels;
		public static readonly IClient< IDataCommand > Data;
		public static readonly IClient< IInventoryCommand > Inventory;
		public static readonly IClient< IReceivingCommand > Receiving;

		static Global()
		{
			var clients = new Clients();

			clients.Initialize( ClientsFactory.CreateClient( BoundedContextsContainer.Foundation, ( viewStore, sender ) => new SimpleClient< IFoundationCommand >( viewStore, sender ), AttributesFactory ) );
			clients.Initialize( ClientsFactory.CreateClient( BoundedContextsContainer.Channels, ( viewStore, sender ) => new SimpleClient< IChannelsCommand >( viewStore, sender ), AttributesFactory ) );
			clients.Initialize( ClientsFactory.CreateClient( BoundedContextsContainer.Data, ( viewStore, sender ) => new SimpleClient< IDataCommand >( viewStore, sender ), AttributesFactory ) );
			clients.Initialize( ClientsFactory.CreateClient( BoundedContextsContainer.Inventory, ( viewStore, sender ) => new SimpleClient< IInventoryCommand >( viewStore, sender ), AttributesFactory ) );
			clients.Initialize( ClientsFactory.CreateClient( BoundedContextsContainer.Receiving, ( viewStore, sender ) => new SimpleClient< IReceivingCommand >( viewStore, sender ), AttributesFactory ) );

			Foundation = clients.Foundation;
			Channels = clients.Channels;
			Data = clients.Data;
			Inventory = clients.Inventory;
			Receiving = clients.Receiving;
		}

		private static MessageAttribute[] AttributesFactory()
		{
			return new[] { new MessageAttribute( "admin-user", "console" ) };
		}
	}
}