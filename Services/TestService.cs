using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Exceptions;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using System.Security.Claims;

namespace Ces_Platform_Server_Side.Services
{
    public class TestService(ITestRepository repository, ILoggerWrapper<User> logger, IHttpContextAccessor accessor) : ITestService
    {
        public async Task<TestResponse> CreateApprovedTest(CreateTestRequest request, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            Test test = Test.Create(request,username!);

            test.Status = TestStatus.Approved;

            if (await repository.AddTestAsync(test, ct))
            {
                Test? Fulltest = await repository.GetTestByIdAsync(test.Id, ct);
                logger.LogInformation($"Create new Approced Test test Id {test.Id} by {username} at {test.CreatedAtUtc}", userRole);

                return TestResponse.FromModel(Fulltest!);
            }
            logger.LogWarning($"Error occured while adding an Approved test testId {test.Id} by {username} at {test.CreatedAtUtc};", userRole);
            throw new InvalidOperationException("Error occured while adding the test");
        }

        public async Task<TestResponse> CreatePendingTest(CreateTestRequest request, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            Test test = Test.Create(request, username!);

            if (await repository.AddTestAsync(test, ct))
            {
                Test? Fulltest = await repository.GetTestByIdAsync(test.Id, ct);
                logger.LogInformation($"Create new Approced Test test Id {test.Id} by {username} at {test.CreatedAtUtc}", userRole);

                return TestResponse.FromModel(Fulltest!);
            }
            logger.LogWarning($"Error occured while adding an Approved test testId {test.Id} by {username} at {test.CreatedAtUtc};", userRole);

            throw new InvalidOperationException("Error occured while adding the test");
        }

        public async Task DeleteTest(Guid TestId, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            if (!await repository.DeleteTestAsync(TestId, ct)) 
            {
                logger.LogError($"Error occured while deleting the Test {TestId} at {DateTime.Now} by {username}", userRole);
                throw new InvalidOperationException("Error occurd while deleting the Test");
            }
            logger.LogInformation($"deleted test {TestId} by {username} at {DateTime.UtcNow}", userRole);
        }

        public async Task<PagedResult<TestPageResponse>> GetPagedTests(TestFilter? filter, CancellationToken ct = default)
        {
            (int totalTests, List<Test> tests) = await repository.GetTestsPageAsync(filter, ct);

            filter ??= new();

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
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            Test? test =  await repository.GetTestByIdAsync(TestId,ct);

            if (test is null)
            {
                logger.LogWarning($"User {TestId} not found at {DateTime.Now} requested by {username}", userRole);
                throw new BusinessRuleException("test not found", StatusCodes.Status404NotFound);
            }
            return TestResponse.FromModel(test);
        }

        public async Task UpdateTest(Guid TestId, UpdateTestRequest request, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            Test? test = await repository.GetTestByIdAsync(TestId,ct);

            if (test is null)
            {
                logger.LogWarning($"test {TestId} not found at {DateTime.Now} requested by {username}", userRole);

                throw new BusinessRuleException("test not found", StatusCodes.Status404NotFound);
            }
            if (test.IsEqual(request))
                return;

            test.Assign(request,username!);
            
            if(!await repository.UpdateTestAsync(ct))
            {
                logger.LogError($"Error occured while updating the test {test.Id} at {DateTime.Now} by {username}", userRole);

                throw new InvalidOperationException("Error occured while updating the test");
            }
            logger.LogInformation($"updated test {test.Id} activation by {username} at {test.LastModifiedAtUtc}", userRole);

        }

        public async Task UpdateTestStatus(Guid testId, UpdateTestStatusRequest request, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            var test = await repository.GetTestByIdAsync(testId, ct) ?? throw new BusinessRuleException("Test not found", StatusCodes.Status404NotFound);

            if(test is null)
            {
                logger.LogWarning($"test {testId} not found at {DateTime.Now} requested by {username}", userRole);
                throw new BusinessRuleException("test not found", StatusCodes.Status404NotFound);
            }
            if (test.Status == request.Status)
                return;

            test.Status = request.Status;

            if (!await repository.UpdateTestAsync(ct))
            {
                logger.LogError($"Error occured while updating the test status {test.Id} at {DateTime.Now} by {username}", userRole);

                throw new InvalidOperationException("Error occured while updating the test activation");
            }
            logger.LogInformation($"updated test status {test.Id} activation by {username} at {test.LastModifiedAtUtc}", userRole);

        }
    }
}
