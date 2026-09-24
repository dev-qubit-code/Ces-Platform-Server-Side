using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;

namespace Ces_Platform_Server_Side.Interfaces
{
    public interface ITestService
    {
        public Task<TestResponse> CreateTest(CreateTestRequest request, CancellationToken ct = default);
        public Task UpdateTest(Guid TestId, UpdateTestRequest request, CancellationToken ct = default);
        public Task<PagedResult<TestPageResponse>> GetPagedTests(TestFilter? filter, CancellationToken ct = default);
        public Task<TestResponse> GetTestById(Guid TestId, CancellationToken ct);
        public Task DeleteTest(Guid TestId, CancellationToken ct = default);
    }
}

