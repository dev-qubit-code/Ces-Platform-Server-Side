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
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);


            if (request.CourseId == default(Guid))
                throw new ArgumentNullException("CourseId Is Required");

               if (request.TeacherId== default(Guid))
                throw new ArgumentNullException("TeacherId Is Required");

            Note CreatedNote = Note.Create(request,username!);

            if (await repository.AddNoteAsync(CreatedNote, ct))
            {
                NoteResponse AddedNote = await GetNoteById(CreatedNote.Id, ct);
                
                logger.LogInformation($"Create new Note {CreatedNote.Id} by {username} at {CreatedNote.CreatedAtUtc}", userRole);

                return AddedNote;
            }
            logger.LogError($"Error occured while adding a new Note {CreatedNote.Id} by {username} at {DateTime.Now}", userRole);

            throw new InvalidOperationException("Error occured while adding the note");
        }
        public async Task DeleteNote(Guid NoteId, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            var sucsses = await repository.DeleteNoteAsync(NoteId, ct);
            if (sucsses)
            {
                logger.LogInformation($"Delete Note {NoteId} by {username} at {DateTime.Now}", userRole);
            }
            else
            {
                logger.LogError($"Error occured while Delete a Note {NoteId} by {username} at {DateTime.Now}", userRole);
                throw new InvalidOperationException("Error occured while Delete the note");
            }
        }
        public async Task<NoteResponse> GetNoteById(Guid NoteId, CancellationToken ct)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            Note? FoundNote = await repository.GetNoteByIdAsync(NoteId, ct);

            if (FoundNote is null)
                {
                logger.LogWarning($"Note {NoteId} not found at {DateTime.Now} requested by {username}", userRole);

                throw new BusinessRuleException("note not found", StatusCodes.Status404NotFound);
            }
            
                return NoteResponse.FromModel(FoundNote);
            
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
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            if (request.CourseId == default(Guid))
                throw new ArgumentNullException("CourseId Is Required");

            if(request.TeacherId == default(Guid))
                throw new ArgumentNullException("TeacherId Is Required");
            
            var note = await repository.GetNoteByIdAsync(NoteId);
            if (note is null)
            {
                logger.LogWarning($"User {NoteId} not found at {DateTime.Now} requested by {username}", userRole);
                throw new BusinessRuleException("note not found", StatusCodes.Status404NotFound);

            }
            if (note.IsEqual(request))
                return;
            note.Assign(request,username!);
            if (!await repository.UpdateNoteAsync(ct))
            {
                logger.LogInformation($"updated note {note.Id} activation by {username} at {note.LastModifiedAtUtc}", userRole);
                throw new InvalidOperationException("Error occured while updating the note");
            }

        }
    }
}
