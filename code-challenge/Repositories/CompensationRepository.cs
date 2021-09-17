using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.EmployeeId = Guid.NewGuid().ToString();//create new identifier for new compensation
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetById(string id)
        {
            //Get either the singular value that has the same ID or return null, utilizes lambdas to do so
            return _compensationContext.Compensations.SingleOrDefault(c => c.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            //save the changes so that data persists, needs to be done asynchronously as it's dealing with the data access layer (DAL)
            return _compensationContext.SaveChangesAsync();
        }
    }
}
