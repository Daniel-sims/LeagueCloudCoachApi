namespace LccWebAPI.Controllers.Utils.Match
{
    public interface IMatchControllerUtils
    {
        Models.ApiMatch.Match ConvertDbMatchToApiMatch(Models.DbMatch.Match dbMatch);
    }
}
