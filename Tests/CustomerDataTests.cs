using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using SomeBasicNEventStoreApp.Core;
using System.Linq;
using NEventStore;
using System;

namespace SomeBasicNEventStoreApp.Tests
{
	[TestFixture]
	public class CustomerDataTests
	{

		private IStoreEvents _engine;

		[Test]
		public void CanGetCustomerById()
		{
			using (var stream = _engine.OpenStream("Customer_1", 0, int.MaxValue))
			{
				Assert.AreEqual(1, stream.CommittedEvents.Count());
			}
		}

		[Test]
		public void CanGetProductById()
		{
			using (var stream = _engine.OpenStream("Product_1", 0, int.MaxValue))
			{
				Assert.AreEqual(1, stream.CommittedEvents.Count());
			}
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
			_engine = Wireup.Init()
		.UsingInMemoryPersistence()	
		.InitializeStorageEngine()
		.UsingJsonSerialization()
			.Compress()
		// Example of NServiceBus dispatcher: https://gist.github.com/1311195
		//.DispatchTo(new My_NServiceBus_Or_MassTransit_OrEven_WCF_Adapter_Code())
	.Build();
			XmlImport.Parse(XDocument.Load(Path.Combine("TestData", "TestData.xml")), new[] { typeof(Customer), typeof(Order), typeof(Product) },
							(type, obj) =>
							{
								if (obj is Customer) {
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
		}


		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			_engine.Dispose();
		}
	}
}
