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
    public class NoteService(INoteRepository repository ,ILoggerWrapper<User> logger, IHttpContextAccessor accessor) : INoteService
    {
        public async Task<NoteResponse> CreateNote(CreateNoteRequest request, CancellationToken ct = default)
        {
            var username = "طالب";
            var userRole = UserRole.Manager;


            Note CreatedNote = Note.Create(request, username!);

            if (!await repository.AddNoteAsync(CreatedNote, ct))
                throw new InvalidOperationException("Error occured while adding the note");

            NoteResponse AddedNote = await GetNoteById(CreatedNote.Id, ct);

            logger.LogInformation($"تم انشاء ملزمه لمادة: {CreatedNote!.Course.Name} تاريخ الملزمه: {CreatedNote.Date} بواسطة: {username} في تاريخ: {CreatedNote!.CreatedAtUtc}", userRole);


            return AddedNote;
        }
        public async Task DeleteNote(Guid NoteId, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            var note = await repository.GetNoteByIdAsync(NoteId,ct);

            if (note is null)
            {

                throw new BusinessRuleException("note not found", StatusCodes.Status404NotFound);
            }

            if (!await repository.DeleteNoteAsync(note, ct))
                throw new InvalidOperationException("Error occured while Delete the note");
            
            logger.LogInformation($"تم حذف ملزمه لمادة: {note!.Course.Name} تاريخ الملزمه: {note.Date} بواسطة: {username} في تاريخ: {DateTimeOffset.UtcNow}", userRole);

        }
        public async Task<NoteResponse> GetNoteById(Guid NoteId, CancellationToken ct)
        {
       
            Note? FoundNote = await repository.GetNoteByIdAsync(NoteId, ct);

            if (FoundNote is null)
                {

                throw new BusinessRuleException("note not found", StatusCodes.Status404NotFound);
            }
            
                return NoteResponse.FromModel(FoundNote);
            
        }
        public async Task<PagedResult<NotePageResponse>> GetPagedNotes(NoteFilter filter, CancellationToken ct = default)
        {
            (int CountOfItems, List<NotePageResponse> notes) = await repository.GetNotesPageAsync(filter, ct);


            if (notes is null || !notes.Any())
                return PagedResult<NotePageResponse>.Create(
                    [],
                    CountOfItems,
                    filter.Page,
                    filter.PageSize
                );

            return PagedResult<NotePageResponse>.Create(
                notes,
                CountOfItems,
                filter.Page,
                filter.PageSize
                );
        }
        public async Task UpdateNote(Guid NoteId, UpdateNoteRequest request, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);
            
            var note = await repository.GetNoteByIdAsync(NoteId);
            if (note is null)
                throw new BusinessRuleException("note not found", StatusCodes.Status404NotFound);

            if (note.IsEqual(request))
                return;

            note.Assign(request,username!);

            if (!await repository.UpdateNoteAsync(ct))
                throw new InvalidOperationException("Error occured while updating the note");

            logger.LogInformation($"تم تعديل ملزمه لمادة: {note!.Course.Name} تاريخ الملزمه: {note.Date} بواسطة: {username} في تاريخ: {note!.LastModifiedAtUtc}", userRole);


        }
    }
}
