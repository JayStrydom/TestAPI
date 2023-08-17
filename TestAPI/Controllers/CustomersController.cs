using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly PerformanceContext _context;

        public CustomersController(PerformanceContext context)
        {
            _context = context;
        }

        // GET: customers/sync
        [HttpGet("sync")]
        public CustomerInfoModel GetCustomers()
        {
            var model = new CustomerInfoModel();     
            var customers = new List<CustomerModel>();

            // dumb queries to excercise the DB
            model.Total = _context.Customers.Count(c => c.CustomerID != 0);
            var salesOrderTotal = _context.SalesOrders.Count(c => c.Customer.CustomerID == 29485);

            var records = _context.SalesOrders
                            .Include(c => c.Customer)
                            .Where(so => so.SalesOrderID > 0)
                            .OrderByDescending(so => so.OrderDate)
                            .Take(50);

            foreach (var record in records)
            {
                customers.Add(new CustomerModel()
                {
                    CustomerID = record.CustomerID,
                    Title = record.Customer.Title,
                    FirstName = record.Customer.FirstName,
                    MiddleName = record.Customer.MiddleName,
                    LastName = record.Customer.LastName,
                    CompanyName = record.Customer.CompanyName,
                    SalesPerson = record.Customer.SalesPerson,
                    EmailAddress = record.Customer.EmailAddress
                });
            }

            model.Customers = customers;
  
            return model;
        }

        // GET: customers/async
        [HttpGet("async")]
        public async Task<CustomerInfoModel> GetCustomersAsync()
        {
            var model = new CustomerInfoModel();
            var customers = new List<CustomerModel>();

            // dumb queries to excercise the DB
            model.Total = await _context.Customers.CountAsync(c => c.CustomerID != 0);
            var salesOrderTotal = await _context.SalesOrders.CountAsync(c => c.Customer.CustomerID == 29485);

            var records = await _context.SalesOrders
                                    .Include(c => c.Customer)
                                    .Where(so => so.SalesOrderID > 0)
                                    .OrderByDescending(so => so.OrderDate)                            
                                    .Take(50).ToListAsync();

            foreach (var record in records)
            {
                customers.Add(new CustomerModel()
                {
                    CustomerID = record.CustomerID,
                    Title = record.Customer.Title,
                    FirstName = record.Customer.FirstName,
                    MiddleName = record.Customer.MiddleName,
                    LastName = record.Customer.LastName,
                    CompanyName = record.Customer.CompanyName,
                    SalesPerson = record.Customer.SalesPerson,
                    EmailAddress = record.Customer.EmailAddress
                });
            }

            model.Customers = customers;

            return model;
        }
    }
}
