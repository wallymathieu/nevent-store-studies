using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using SomeBasicNEventStoreApp.Core;
using System.Linq;
using NEventStore;
using System;
using MemBus;
using MemBus.Configurators;
using MemBus.Subscribing;
using NEventStore.Dispatcher;
using NEventStore.Client;
using System.Threading.Tasks;

namespace SomeBasicNEventStoreApp.Tests
{
	[TestFixture]
	public class CustomerDataTests
	{
		private IStoreEvents _engine;
		private IRepository _repository;

		[Test]
		public void CanGetCustomerById()
		{
			Assert.IsNotNull(_repository.GetCustomer(1));
		}

		[Test]
		public void CanGetProductById()
		{
			Assert.IsNotNull(_repository.GetProduct(1));
		}

		[SetUp]
		public void Setup()
		{
		}


		[TearDown]
		public void TearDown()
		{
		}

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			var testAdapter = new HardCodedIoc();
			_repository = testAdapter._repository;
			using (var bus = BusSetup.StartWith<Conservative>()
				   .Apply<FlexibleSubscribeAdapter>(a =>
				   {
					   a.ByInterface(typeof(ICommandHandler<>));
				   })
				   //.Apply<IoCSupport>(s => s.SetAdapter(testAdapter).SetHandlerInterface(typeof(ICommandHandler<>)))
				   .Construct())
			{
				foreach (var item in testAdapter.GetAllInstances(typeof(ICommandHandler<>))) { bus.Subscribe(item); }
				//bus.Subscribe()
				_engine = Wireup.Init().UsingInMemoryPersistence().Build();

				var pollingClient = new PollingClient(_engine.Advanced);
				using (var _commitObserver = pollingClient.ObserveFrom(null))
				{
					var hook = new PollingHook(_commitObserver);
					_commitObserver.Subscribe(new BusDispatcher(bus));

					var s = _commitObserver.Start();

					XmlImport.Parse(XDocument.Load(Path.Combine("TestData", "TestData.xml")), new[] { typeof(Customer), typeof(Order), typeof(Product) },
									(type, obj) =>
									{
										if (obj is Customer)
										{
											var o = new AddCustomerCommand { Object = (Customer)obj };
											using (var stream = _engine.CreateStream(o.Object.EventId))
											{
												stream.Add(new EventMessage { Body = o });
												stream.CommitChanges(Guid.NewGuid());
											}

										}
										if (obj is Product)
										{
											var o = new AddProductCommand { Object = (Product)obj };
											using (var stream = _engine.CreateStream(o.Object.EventId))
											{
												stream.Add(new EventMessage { Body = o });
												stream.CommitChanges(Guid.NewGuid());
											}
										}
										if (obj is Order)
										{
											var o = new AddOrderCommand { Object = (Order)obj };
											using (var stream = _engine.CreateStream(o.Object.EventId))
											{
												stream.Add(new EventMessage { Body = o });
												stream.CommitChanges(Guid.NewGuid());
											}
										}
									}, "http://tempuri.org/Database.xsd");
					int count = 150;
					while (!_repository.Any && count-- > 0)
					{
						Task.Delay(100).Wait();
					}
				}
			}
		}


		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			_engine.Dispose();
		}
	}
}
