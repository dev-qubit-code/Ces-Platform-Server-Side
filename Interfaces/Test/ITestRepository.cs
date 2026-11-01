using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Responses;

namespace Ces_Platform_Server_Side.Interfaces
{
    public interface ITestRepository
    {
        public Task<(int, List<TestPageResponse>)> GetTestsPageAsync(TestFilter? filter, CancellationToken ct = default);
        public Task<Test?> GetTestByIdAsync(Guid TestId, CancellationToken ct = default);
        public Task<bool> AddTestAsync(Test Test, CancellationToken ct = default);
        public Task<bool> UpdateTestAsync(CancellationToken ct = default);
        public Task<bool> DeleteTestAsync(Test test, CancellationToken ct = default);
        public Task<int> GetTestsCountAsync(CancellationToken ct = default);
    }
}
