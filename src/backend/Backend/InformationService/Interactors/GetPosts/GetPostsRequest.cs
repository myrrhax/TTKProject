namespace InformationService.Interactors.GetPosts;

public record GetPostsRequest(int page = 1, string query = "", string sortByDate="desc");