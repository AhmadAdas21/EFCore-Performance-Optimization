using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_with_DOCKER.DataBase;
using API_with_DOCKER.services;

namespace API_with_DOCKER.Tests
{
    internal class StockConcurrencyTests
    {
        private readonly AppDbContext _contect;
        private readonly StockService _stockService;


        [Fact]
        public async Task Purchase_WithAvailableStock_ShouldSucceed()
        {

        }
        [Fact]
        public async Task Purchase_100ConcurrentRequests_Only10ShouldSucceed()
        {

        }
        [Fact]
        public async Task 
    }
}
