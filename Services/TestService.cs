using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using SPMS_PROJECT.Exceptions;

namespace Ces_Platform_Server_Side.Services
{
    public class TestService(ITestRepository repository) : ITestService
    {
        public async Task<TestResponse> CreateTest(CreateTestRequest request, CancellationToken ct = default)
        {
            Test test = Test.Create(request, "test");

            if (await repository.AddTestAsync(test, ct))
            {
                Test? Fulltest = await repository.GetTestByIdAsync(test.Id, ct);
                    return TestResponse.FromModel(Fulltest!);
            }
            throw new InvalidOperationException("Error occured while adding the test");
        }

        public async Task DeleteTest(Guid TestId, CancellationToken ct = default)
        {
            if (!await repository.DeleteTestAsync(TestId, ct))
                throw new InvalidOperationException("Error occurd while deleting the Test");
        }

        public async Task<PagedResult<TestPageResponse>> GetPagedTests(TestFilter? filter, CancellationToken ct = default)
        {
            (int totalTests, List<Test> tests) = await repository.GetTestsPageAsync(filter, ct);

            filter = new();

            if (tests is null || !tests.Any())
            {
                return PagedResult<TestPageResponse>.Create(
                    [], 
                    totalTests, 
                    filter.Page, 
                    filter.PageSize);
            }

            return PagedResult<TestPageResponse>.Create(
                TestPageResponse.FromModels(tests),
                totalTests, 
                filter.Page, 
                filter.PageSize);

        }

        public async Task<TestResponse> GetTestById(Guid TestId, CancellationToken ct)
        {
           Test? test =  await repository.GetTestByIdAsync(TestId,ct);
            return test is null ?throw new BusinessRuleException("test not found", StatusCodes.Status404NotFound) : TestResponse.FromModel(test);
        }

        public async Task UpdateTest(Guid TestId, UpdateTestRequest request, CancellationToken ct = default)
        {
            Test? test = await repository.GetTestByIdAsync(TestId,ct);

            if (test is null)
                throw new BusinessRuleException("test not found", StatusCodes.Status404NotFound);

            if (test.IsEqual(request))
                throw new BusinessRuleException("test is allready updated", StatusCodes.Status409Conflict);

            test.Assign(request,"tester");
            await repository.UpdateTestAsync(ct);
        }
    }
}
