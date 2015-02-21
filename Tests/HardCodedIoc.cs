using System;
using System.Collections.Generic;
using MemBus;
using SomeBasicNEventStoreApp.Core;

namespace SomeBasicNEventStoreApp.Tests
{
	internal class HardCodedIoc : IocAdapter
	{
		private object[] handlers;
		public readonly IRepository _repository = new Repository();
		public HardCodedIoc()
		{
			handlers = new object[] {
				new AddCustomerCommandHandler(_repository),
				new AddOrderCommandHandler(_repository),
				new AddProductCommandHandler(_repository)
			};
        }


		public IEnumerable<object> GetAllInstances(Type desiredType)
		{
			return handlers;
		}
	}
}