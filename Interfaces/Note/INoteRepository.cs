using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.FIlters.QueryFilters;

namespace Ces_Platform_Server_Side.Interfaces
{
    public interface INoteRepository
    {
        public Task<(int, List<Note>)> GetNotesPageAsync(NoteFilter? filter, CancellationToken ct = default);
        public Task<Note?> GetNoteByIdAsync(Guid NoteId, CancellationToken ct = default);
        public Task<bool> AddNoteAsync(Note Note, CancellationToken ct = default);
        public Task<bool> UpdateNoteAsync(CancellationToken ct = default);
        public Task<bool> DeleteNoteAsync(Guid NoteId, CancellationToken ct = default);
        public Task<int> GetNotesCountAsync(CancellationToken ct = default);
    }
}

