using AuthService.Entities;
using AuthService.Utils;

namespace AuthService.Interactors.GetUsers;

public record GetUsersParams(int Page = 1,
    SortOptions? SortByLogin = SortOptions.ASCENDING,
    SortOptions? SortByFullname = SortOptions.ASCENDING, 
    Role? SortByRole = null,
    SortOptions? SortByRegistrationDate = SortOptions.ASCENDING);