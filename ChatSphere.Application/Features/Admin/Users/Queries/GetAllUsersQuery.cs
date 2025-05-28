using ChatSphere.Domain.DTOs;
using MediatR;

namespace ChatSphere.Application.Features.Admin.Users.Queries;

public class GetAllUsersQuery : IRequest<List<UserDto>> { }
