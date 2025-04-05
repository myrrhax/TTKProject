using InformationService.Utils;

namespace InformationService.Interactors.GetHistory;

public record GetHistoryParams(int Page = 1, string Query = "", DateSortType DateSortType = DateSortType.Descending);