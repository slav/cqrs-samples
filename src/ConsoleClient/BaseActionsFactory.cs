using System.Collections.Generic;
using Abstractions;
using Foundation.Contracts.ValueObjects;
using Foundation.Contracts.ValueObjects.Tenant;
using Foundation.Contracts.ValueObjects.User;
using Cqrs.ConsoleClient.Actions;

namespace Cqrs.ConsoleClient
{
	public abstract class BaseActionsFactory
	{
		public abstract IEnumerable< IAction > CreateCommandsToExecute();

		protected CommandSender< TCommand > Send< TCommand >( TCommand command ) where TCommand : ICommand
		{
			return new CommandSender< TCommand >( command );
		}

		protected ViewGetter< TView, TCommand > View< TView, TCommand >( object key = null ) where TCommand : ICommand where TView : new()
		{
			return new ViewGetter< TView, TCommand >( key );
		}

		protected RefIds RefIds( long tenantId, long userId )
		{
			return new RefIds( new TenantId( tenantId ), new UserId( userId ) );
		}
	}
}