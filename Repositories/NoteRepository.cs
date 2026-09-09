using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Microsoft.EntityFrameworkCore;
using SPMS_PROJECT.Exceptions;

namespace Ces_Platform_Server_Side.Repositories
{
    public class NoteRepository(AppDbContext context) : INoteRepository
    {
        public async Task<bool> AddNoteAsync(Note Note, CancellationToken ct = default)
        {
            await context.Notes.AddAsync(Note,ct);
            
            return await  context.SaveChangesAsync(ct) > 0  ;
        }
        public async Task<bool> DeleteNoteAsync(Guid NoteId, CancellationToken ct = default)
        {
            var note = await context.Notes.FindAsync(NoteId, ct);

            if (note is null)
                return false;

            context.Notes.Remove(note);
            return await context.SaveChangesAsync() > 0;

        }
        public async Task<Note?> GetNoteByIdAsync(Guid NoteId, CancellationToken ct = default)
        {
            Note? note = await context.Notes
                .Include(t => t.Teacher)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(n => n.Id == NoteId, ct);

            return note; 
         
        }
        public async Task<int> GetNotesCountAsync(CancellationToken ct = default)
        => await context.Notes.CountAsync(ct);
       
        public async Task<(int, List<Note>)> GetNotesPageAsync(NoteFilter? filter, CancellationToken ct = default)
        {

            IQueryable<Note> notes = context.Notes;

            int CountOfAllItems;
            List<Note>PageItem;

            if(filter is null)
            {
                PageItem = await context.Notes.Include(t => t.Teacher).Include(c => c.Course).Take(10).ToListAsync(ct);
                CountOfAllItems = await context.Notes.CountAsync(ct);

                return (CountOfAllItems, PageItem);
            }

            filter.Page = Math.Max(1, filter.Page);
            filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);

            if (!string.IsNullOrWhiteSpace(filter.Search))
                {
                notes = notes.Where(n => n.Name.Contains(filter.Search));
                CountOfAllItems = await context.Notes.CountAsync(ct);
            }
            CountOfAllItems = await context.Notes.CountAsync(ct);
            
            PageItem = await notes.Include(t => t.Teacher).Include(c => c.Course).Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .ToListAsync(ct);
            
            return (CountOfAllItems, PageItem);

        }
        public async Task<bool> UpdateNoteAsync(CancellationToken ct = default) 
            => await context.SaveChangesAsync(ct) > 0 ;
    }
}
