using AutoMapper;
using TaskManagementAPI.DTOs.Request;
using TaskManagementAPI.DTOs.Response;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Mappings
{
    // Mappings/TaskManagementProfile.cs
    public class TaskManagementProfile : Profile
    {
        public TaskManagementProfile()
        {
            // Task mappings
            CreateMap<CreateTaskDto, Models.Task>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(_ => false));

            CreateMap<Models.Task, TaskResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<UpdateTaskDto, Models.Task>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Comment mappings
            CreateMap<CreateCommentDto, TaskComment>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<TaskComment, CommentResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));
            CreateMap<TaskComment, TaskCommentResponseDto>()
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));
            // User mappings
            //   CreateMap<User, UserResponseDto>();
        }
    }
}
