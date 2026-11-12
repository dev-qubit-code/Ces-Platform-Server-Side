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

            Test test = Test.Create(request, username!);

            test.Status = TestStatus.Approved;

            if (!await repository.AddTestAsync(test, ct))
                throw new InvalidOperationException("Error occured while adding the test");

            Test? Fulltest = await repository.GetTestByIdAsync(test.Id, ct)!;
            logger.LogInformation($"تم انشاء نموذج اختبار لمادة: {Fulltest!.Course.Name} تاريخ النموذج: {test.Date} بواسطة: {username} في تاريخ: {Fulltest!.CreatedAtUtc} الحاله: {Fulltest.Status}", userRole);

            return TestResponse.FromModel(Fulltest!);
        }

        public async Task<TestResponse> CreatePendingTest(CreateTestRequest request, CancellationToken ct = default)
        {
            var username = "طالب";
            var userRole = UserRole.Manager;

            Test test = Test.Create(request, username!);

            if (!await repository.AddTestAsync(test, ct))
                throw new InvalidOperationException("Error occured while adding the test");

            Test? Fulltest = await repository.GetTestByIdAsync(test.Id, ct)!;
            logger.LogInformation($"تم انشاء نموذج اختبار لمادة: {Fulltest!.Course.Name} تاريخ النموذج: {test.Date} بواسطة: {username} في تاريخ: {Fulltest!.CreatedAtUtc} الحاله: {Fulltest.Status}", userRole);

            return TestResponse.FromModel(Fulltest!);
        }

        public async Task DeleteTest(Guid TestId, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            var test = await repository.GetTestByIdAsync(TestId, ct);
            if (test is null) 
                throw new BusinessRuleException("test not found", StatusCodes.Status404NotFound);

            if (!await repository.DeleteTestAsync(test, ct)) 
                throw new InvalidOperationException("Error occurd while deleting the Test");

            logger.LogInformation($"تم حذف نموذج اختبار لمادة: {test!.Course.Name} تاريخ النموذج: {test.Date} بواسطة: {username} في تاريخ: {DateTimeOffset.UtcNow} الحاله: {test.Status}", userRole);

        }

        public async Task<PagedResult<TestPageResponse>> GetPagedTests(TestFilter filter, CancellationToken ct = default)
        {
            (int totalTests, List<TestPageResponse> tests) = await repository.GetTestsPageAsync(filter, ct);

            if (tests is null || !tests.Any())
            {
                return PagedResult<TestPageResponse>.Create(
                    [], 
                    totalTests, 
                    filter.Page, 
                    filter.PageSize);
            }

            return PagedResult<TestPageResponse>.Create(
                tests,
                totalTests, 
                filter.Page, 
                filter.PageSize);

        }

        public async Task<TestResponse> GetTestById(Guid TestId, CancellationToken ct)
        {
            Test? test =  await repository.GetTestByIdAsync(TestId,ct);

            if (test is null)
            {
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
                throw new BusinessRuleException("test not found", StatusCodes.Status404NotFound);

            if (test.IsEqual(request))
                return;

            test.Assign(request,username!);
            
            if(!await repository.UpdateTestAsync(ct))
                throw new InvalidOperationException("Error occured while updating the test");

            logger.LogInformation($"تم تعديل نموذج اختبار لمادة: {test!.Course.Name} تاريخ النموذج: {test.Date} بواسطة: {username} في تاريخ: {test!.LastModifiedAtUtc} الحاله: {test.Status}", userRole);


        }

        public async Task UpdateTestStatus(Guid testId, UpdateTestStatusRequest request, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            var test = await repository.GetTestByIdAsync(testId, ct) ?? throw new BusinessRuleException("Test not found", StatusCodes.Status404NotFound);

            if(test is null)
                throw new BusinessRuleException("test not found", StatusCodes.Status404NotFound);

            if (test.Status == request.Status)
                return;

            test.Status = request.Status;

            if (!await repository.UpdateTestAsync(ct))
                throw new InvalidOperationException("Error occured while updating the test activation");

            logger.LogInformation($"تم تعديل حالة نموذج الاختبار لمادة: {test!.Course.Name} تاريخ النموذج: {test.Date} بواسطة: {username} في تاريخ: {test!.LastModifiedAtUtc} الحاله: {test.Status}", userRole);

        }

        public async Task<List<int>> GetExistingYears(CancellationToken ct = default) => await repository.GetExistingYears(ct);
    }
}
