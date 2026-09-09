using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using SPMS_PROJECT.Exceptions;

namespace Ces_Platform_Server_Side.Services
{
    public class NoteService(INoteRepository repository) : INoteService
    {
        public async Task<NoteResponse> CreateNote(CreateNoteRequest request, CancellationToken ct = default)
        {
            Note CreatedNote = Note.Create(request, "Test");
            if (await repository.AddNoteAsync(CreatedNote, ct))
            {
                NoteResponse AddedNote = await GetNoteById(CreatedNote.Id,ct);
                return AddedNote;
            }
            throw new InvalidOperationException("Error occured while adding the note");
        }
        public async Task DeleteNote(Guid NoteId, CancellationToken ct = default)
        {
            var sucsses = await repository.DeleteNoteAsync(NoteId, ct);
            if(!sucsses)
                throw new InvalidOperationException("Error occured while Delete the note");
        }
        public async Task<NoteResponse> GetNoteById(Guid NoteId, CancellationToken ct)
        {
            Note? FoundNote = await repository.GetNoteByIdAsync(NoteId,ct);

            return FoundNote is not null? NoteResponse.FromModel(FoundNote) : throw new BusinessRuleException("note not found", StatusCodes.Status404NotFound); 
        }
        public async Task<PagedResult<NotePageResponse>> GetPagedNotes(NoteFilter? filter, CancellationToken ct = default)
        {
            (int CountOfItems, List<Note> notes) = await repository.GetNotesPageAsync(filter, ct);

            filter ??= new();

            if (notes is null || !notes.Any())
                return PagedResult<NotePageResponse>.Create(
                    [],
                    CountOfItems,
                    filter.Page,
                    filter.PageSize
                );

            return PagedResult<NotePageResponse>.Create(
                NotePageResponse.FromModels(notes),
                CountOfItems,
                filter.Page,
                filter.PageSize
                );
        }
        public async Task UpdateNote(Guid NoteId, UpdateNoteRequest request, CancellationToken ct = default)
        {
            var note = await repository.GetNoteByIdAsync(NoteId);
            if (note is null)
                throw new BusinessRuleException("note not found", StatusCodes.Status404NotFound);
            if (note.IsEqual(request))
                throw new BusinessRuleException("this note is all ready Updated",StatusCodes.Status409Conflict);
            note.Assign(request, "tester");
            if(!await repository.UpdateNoteAsync(ct))
                throw new InvalidOperationException("Error occured while updating the note");

        }
    }
}
