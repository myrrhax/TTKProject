using InformationService.Utils;

namespace InformationService.Interactors.GetPosts;

public record GetPostsParams(int Page = 1, string Query = "", DateSortType DateSortType = DateSortType.Descending);