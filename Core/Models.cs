using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeBasicNEventStoreApp.Core
{
	public class AddCustomerCommandHandler : ICommandHandler<AddCustomerCommand>
	{
		private readonly IRepository _repository;
		public AddCustomerCommandHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(AddCustomerCommand command)
		{
			_repository.Save(command.Object);
		}
	}
	public class AddProductCommandHandler : ICommandHandler<AddProductCommand>
	{
		private readonly IRepository _repository;
		public AddProductCommandHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(AddProductCommand command)
		{
			_repository.Save(command.Object);
		}
	}
	public class AddOrderCommandHandler : ICommandHandler<AddOrderCommand>
	{
		private readonly IRepository _repository;
		public AddOrderCommandHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(AddOrderCommand command)
		{
			_repository.Save(command.Object);
		}
	}
	[Serializable]
	public class AddCustomerCommand 	{
		public Customer Object { get; set; }
	}
	[Serializable]
	public class AddProductCommand 
	{
		public Product Object { get; set; }
	}
	[Serializable]
	public class AddOrderCommand 
	{
		public Order Object { get; set; }
	}
}
