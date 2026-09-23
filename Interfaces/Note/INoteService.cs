using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.FIlters.QueryFilters;

namespace Ces_Platform_Server_Side.Interfaces
{
    public interface INoteService
    {
        public Task<NoteResponse> CreateNote(CreateNoteRequest request, CancellationToken ct = default);
        public Task UpdateNote(Guid NoteId, UpdateNoteRequest request, CancellationToken ct = default);
        public Task<PagedResult<NotePageResponse>> GetPagedNotes(NoteFilter? filter, CancellationToken ct = default);
        public Task<NoteResponse> GetNoteById(Guid NoteId, CancellationToken ct);
        public Task DeleteNote(Guid NoteId, CancellationToken ct = default);
    }
}
