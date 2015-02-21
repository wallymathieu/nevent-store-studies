using System;
using MemBus;
using NEventStore;
using NEventStore.Client;

namespace SomeBasicNEventStoreApp.Core
{
	public class BusDispatcher : IObserver<ICommit>
	{
		private IBus bus;

		public BusDispatcher(IBus bus)
		{
			this.bus = bus;
		}

		public void OnCompleted()
		{
			//throw new NotImplementedException();
		}

		public void OnError(Exception error)
		{
			//throw new NotImplementedException();
		}

		public void OnNext(ICommit value)
		{
			foreach (var @event in value.Events)
			{
				bus.Publish(@event.Body);
			}
		}
	}
  public class PollingHook : PipelineHookBase
  {
    private readonly IObserveCommits _commitsObserver;

    public PollingHook(IObserveCommits commitsObserver)
    {
      _commitsObserver = commitsObserver;
    }

    public override void PostCommit(ICommit committed)
    {
      base.PostCommit(committed);
      _commitsObserver.PollNow();
    }
  }
}