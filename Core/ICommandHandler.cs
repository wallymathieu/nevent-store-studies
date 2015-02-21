namespace SomeBasicNEventStoreApp.Core
{
	public interface ICommandHandler<T>
	{
		void Handle(T command);
	}
}