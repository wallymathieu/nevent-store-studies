using System.Collections.Generic;

namespace SomeBasicNEventStoreApp.Core
{
	public interface IRepository
	{
		Customer GetCustomer(int v);
		Product GetProduct(int v);
		IEnumerable<Customer> QueryOverCustomers();
		IEnumerable<Product> QueryOverProducts();
		void Save(Product obj);
		void Save(Order obj);
		void Save(Customer obj);
	}
}