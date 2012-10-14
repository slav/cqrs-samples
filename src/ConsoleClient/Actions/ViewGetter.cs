using System;
using Abstractions;
using Lokad.Cqrs;
using Netco.Logging;
using ServiceStack.Text;
using Cqrs.Infrastructure.Client;

namespace Cqrs.ConsoleClient.Actions
{
	public class ViewGetter< TView, TCommand > : IAction where TCommand : ICommand where TView : new()
	{
		public object Key { get; private set; }
		protected IClient< TCommand > Client { get; set; }
		public bool WillUpdateData{ get{ return false; }}
		
		public ViewGetter( object key = null )
		{
			this.Key = key;
		}

		public ViewGetter< TView, TCommand > From( IClient< TCommand > client )
		{
			this.Client = client;
			return this;
		}

		public void Execute()
		{
			this.Log().Info( this.ToString() + ":" + Environment.NewLine + this.GetView() );
		}

		public string GetView()
		{
			Maybe< TView > possibleView;
			if( this.Key != null )
				possibleView = this.Client.GetView< TView >( this.Key );
			else
				possibleView = this.Client.GetSingleton< TView >();

			if( possibleView.HasValue )
				return JsvFormatter.Format( TypeSerializer.SerializeToString( possibleView.Value ) );
			else
				return "View does not exist";
		}

		public override string ToString()
		{
			if( this.Key != null )
				return "View: {0}; Key: {1}".FormatWith( typeof( TView ).Name, this.Key.ToString() );
			else
				return "View: {0}".FormatWith( typeof( TView ).Name );
		}
	}
}