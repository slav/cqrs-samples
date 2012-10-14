namespace Cqrs.ConsoleClient.Actions
{
	public interface IAction
	{
		bool WillUpdateData{ get; }
		void Execute(); 
	}
}