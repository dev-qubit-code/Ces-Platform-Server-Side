using Ces_Platform_Server_Side.FIlters.QueryFilters;

namespace Ces_Platform_Server_Side.Interfaces 
{
    public interface ICourseRepository
    {
      public Task<(int, List<Models.Course>)> GetCoursePageAsync(CourseFilter? filter, CancellationToken ct = default);
        public Task<Models.Course?> GetCourseByIdAsync(Guid Id, CancellationToken ct = default);
        public Task<bool> AddCourseAsync(Models.Course course, CancellationToken ct = default);
        public Task<bool> UpdateCourseAsync(CancellationToken ct = default);
        public Task<bool> DeleteCourseAsync(Guid CourseId, CancellationToken ct = default);
        public Task<int> GetCountAsync(Models.Course course, CancellationToken ct = default);

    }
}
