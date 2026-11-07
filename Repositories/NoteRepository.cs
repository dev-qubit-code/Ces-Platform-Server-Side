using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Responses;
using Microsoft.EntityFrameworkCore;

namespace Ces_Platform_Server_Side.Repositories
{
    public class NoteRepository(AppDbContext context) : INoteRepository
    {
        public async Task<bool> AddNoteAsync(Note Note, CancellationToken ct = default)
        {
            await context.Notes.AddAsync(Note, ct);

            return await context.SaveChangesAsync(ct) > 0;
        }
        public async Task<bool> DeleteNoteAsync(Note note, CancellationToken ct = default)
        {
            context.Notes.Remove(note);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<Note?> GetNoteByIdAsync(Guid NoteId, CancellationToken ct = default)
        {
            Note? note = await context.Notes
                .Include(n => n.Teacher)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(n => n.Id == NoteId, ct);

            return note;

        }
        public async Task<int> GetNotesCountAsync(CancellationToken ct = default)
        => await context.Notes.CountAsync(ct);

        public async Task<(int, List<NotePageResponse>)> GetNotesPageAsync(NoteFilter filter, CancellationToken ct = default)
        {

            IQueryable<Note> notes = context.Notes;

            int CountOfAllItems;
            List<NotePageResponse> PageItem;

            filter.Page = Math.Max(1, filter.Page);
            filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);

            notes = ApplyFilters(notes, filter);

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                notes = ApplySearchTerm(notes, filter.Search);
            }

            notes = ApplySorting(notes, filter.SortColumn, filter.SortDirection);

            CountOfAllItems = await notes.CountAsync(ct);

            PageItem = await notes.Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .Select(n => new NotePageResponse
                          {
                              Id = n.Id,
                              Name = n.Name,
                              Date = n.Date,
                              CourseName = n.Course.Name,
                              TeacherName = n.Teacher.Name,
                          }).ToListAsync(ct);

            return (CountOfAllItems, PageItem);

        }
        public async Task<bool> UpdateNoteAsync(CancellationToken ct = default)
            => await context.SaveChangesAsync(ct) > 0;

        private static IQueryable<Note> ApplyFilters(IQueryable<Note> query,NoteFilter filter)
        {
            var courseNameNormalized = filter.CourseName.Trim().ToLower();
            var teacherNameNormalized = filter.TeacherName.Trim().ToLower();
            var thisYear = DateTimeOffset.UtcNow.Year;

            if(!string.IsNullOrWhiteSpace(courseNameNormalized))
                query = query.Where(n => n.Course.Name.ToLower() == courseNameNormalized);

            if(!string.IsNullOrWhiteSpace(teacherNameNormalized))
                query = query.Where(n => n.Teacher.Name.ToLower() == teacherNameNormalized);

            if(filter.Year != 0)
                query = query.Where(n => n.Date.Year == filter.Year);

            return query;
        }

        private static IQueryable<Note> ApplySearchTerm(IQueryable<Note> query, string searchTerm)
        {      
            var normalized = searchTerm.Trim().ToLower();

            if(!string.IsNullOrWhiteSpace(normalized))
                query = query.Where(n => n.Name.ToLower().Contains(normalized));

            return query;
        }

        private static IQueryable<Note> ApplySorting(IQueryable<Note> query, string sortColumn, string sortDirection)
        {
            var isDescending = sortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase);

            return sortColumn.ToLower() switch
            {
                "createdatutc" => isDescending ? query.OrderByDescending(n => n.CreatedAtUtc) : query.OrderBy(n => n.CreatedAtUtc),
                "name" => isDescending ? query.OrderByDescending(n => n.Name) : query.OrderBy(n => n.Name),
                "teachername" => isDescending ? query.OrderByDescending(n => n.Teacher.Name) : query.OrderBy(n => n.Teacher.Name),
                "coursename" => isDescending ? query.OrderByDescending(n => n.Course.Name) : query.OrderBy(n => n.Course.Name),
                _ => query.OrderByDescending(n => n.CreatedAtUtc)
            };
        }
    }
}
